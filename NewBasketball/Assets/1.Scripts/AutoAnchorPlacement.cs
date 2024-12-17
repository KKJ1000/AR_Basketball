using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AutoAnchorPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab; // 배치할 오브젝트 프리팹

    private ARAnchorManager anchorManager; // ARAnchorManager
    private ARPlaneManager planeManager;   // ARPlaneManager

    [SerializeField]
    private Ball ballScript;

    [SerializeField]
    [Tooltip("게임 시작 시 활성화되는 가이드 텍스트")]
    private Text guideText;

    [SerializeField]
    [Tooltip("바닥을 인식 시킨 후 활성화 되는 가이드 텍스트")]
    private Text guideText1;

    [SerializeField]
    [Tooltip("화면 스와이프 가이드 이미지")]
    private GameObject swipeGuide;

    private bool isGuideTextShown = false; //가이드 텍스트가 이미 보였는지 체크하는 플래그
    private bool isFloorDetected = false;  //바닥이 인식됐는지 체크하는 플래그

    void Start()
    {
        isGuideTextShown = false;
        isFloorDetected = false;
        guideText.gameObject.SetActive(true);   //첫번째 가이드 텍스트 활성화
        guideText1.gameObject.SetActive(false); //두번째 가이드 텍스트 비활성화
        swipeGuide.SetActive(false);            //스와이프 가이드 비활성화

        // ARAnchorManager와 ARPlaneManager 가져오기
        anchorManager = FindObjectOfType<ARAnchorManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();

        if (anchorManager == null || planeManager == null)
        {
            Debug.LogError("ARAnchorManager 또는 ARPlaneManager를 찾을 수 없습니다.");
        }

        // AR Plane이 생성되면 호출되는 이벤트 등록
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        if (planeManager != null)
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        //바닥이 이미 한번 인식되었으면 처리하지 않음
        if (isFloorDetected) return;

        // 새로 감지된 평면만 처리
        foreach (ARPlane plane in args.added)
        {
            CreateAnchorAtPlaneCenter(plane);
            
            if (!isGuideTextShown) //가이드 텍스트가 이미 표시되지 않았을 때만 실행
            {
                guideText.gameObject.SetActive(false); //바닥을 인식하면 가이드 텍스트 비활성화

                guideText1.gameObject.SetActive(true);  //바닥을 인식하면 가이드 텍스트1 활성화

                isGuideTextShown = true; //가이드 텍스트가 표시되었을 때 true로 변경하여 다시는 텍스트가 켜지지 않게 설정.
                StartCoroutine(DisableGuideTextAndShowSwipeGuide());
            }

            // 바닥이 인식되었다.
            isFloorDetected = true;
        }
    }

    IEnumerator DisableGuideTextAndShowSwipeGuide()
    {
        //가이드 텍스트1이 활성화됐을 때 2초 뒤에 비활성화 시키고 또 1.5초뒤 스와이프 가이드 활성화
        yield return new WaitForSeconds(2f);
        guideText1.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        swipeGuide.SetActive(true);
    }

    private void CreateAnchorAtPlaneCenter(ARPlane plane)
    {
        // 평면의 중심 위치 가져오기
        Vector3 planeCenter = plane.center;
        Vector3 createPos = new Vector3(planeCenter.x, planeCenter.y - 2, planeCenter.z + 5);
        Pose anchorPose = new Pose(createPos, Quaternion.identity);

        GameObject instantiatedObject = Instantiate(objectPrefab, anchorPose.position, anchorPose.rotation);

        // 자식 오브젝트 가져오기 (예: "ChildObjectName"이라는 이름의 자식 오브젝트)
        Transform childTransform = instantiatedObject.transform.Find("ShootPoint");

        if (childTransform != null)
        {
            Vector3 childWorldPosition = childTransform.position;

            // Ball 스크립트의 shootPoint 업데이트
            if (ballScript != null)
            {
                ballScript.UpdateShootPoint(childWorldPosition);
                Debug.Log($"ShootPoint 위치가 업데이트되었습니다: {childWorldPosition}");

                // 첫 번째 공 생성
                ballScript.CreateNewBall();
            }
        }
        else
        {
            Debug.LogWarning("ShootPoint 자식 오브젝트를 찾을 수 없습니다.");
        }
    }
}
