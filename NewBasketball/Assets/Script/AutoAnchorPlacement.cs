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
    [Tooltip("게임 시작 시 활성화되는 가이드 텍스트")]
    private Text guideText;

    [SerializeField]
    [Tooltip("바닥을 인식 시킨 후 활성화 되는 가이드 텍스트")]
    private Text guideText1;

    [SerializeField]
    [Tooltip("화면 스와이프 가이드 이미지")]
    private GameObject swipeGuide;

    private float delay = 5.0f; //가이드 텍스트 비활성화 딜레이

    void Start()
    {
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
        // 새로 감지된 평면만 처리
        foreach (ARPlane plane in args.added)
        {
            CreateAnchorAtPlaneCenter(plane);
            guideText.gameObject.SetActive(false); //바닥을 인식하면 가이드 텍스트 비활성화
            guideText.gameObject.SetActive(true);  //바닥을 인식하면 가이드 텍스트1 활성화

            StartCoroutine(DisableGuideTextAfterDelay(delay));
        }
    }

    IEnumerator DisableGuideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); //delay 만큼 대기
        guideText1.gameObject.SetActive(false); //두번째 가이드 텍스트 비활성화
        yield return new WaitForSeconds(2f);    //2초 뒤 스와이프 가이드 활성화
        swipeGuide.SetActive(true);             //스와이프 가이드 활성화
    }

    private void CreateAnchorAtPlaneCenter(ARPlane plane)
    {
        // 평면의 중심 위치 가져오기
        Vector3 planeCenter = plane.center;
        Vector3 createPos = new Vector3(planeCenter.x, planeCenter.y - 2, planeCenter.z + 5);
        Pose anchorPose = new Pose(createPos, Quaternion.identity);

        Instantiate(objectPrefab, anchorPose.position, anchorPose.rotation);
    }
}
