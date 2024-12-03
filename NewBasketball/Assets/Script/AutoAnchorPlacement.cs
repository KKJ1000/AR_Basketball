using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AutoAnchorPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab; // 배치할 오브젝트 프리팹

    private ARAnchorManager anchorManager; // ARAnchorManager
    private ARPlaneManager planeManager;   // ARPlaneManager

    void Start()
    {
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
        }
    }

    private void CreateAnchorAtPlaneCenter(ARPlane plane)
    {
        // 평면의 중심 위치 가져오기
        Vector3 planeCenter = plane.center;
        Vector3 createPos = new Vector3(planeCenter.x,planeCenter.y - 2,planeCenter.z + 5);
        Pose anchorPose = new Pose(createPos, Quaternion.identity);

        // 앵커 생성
        /*ARAnchor anchor = anchorManager.AddAnchor(anchorPose);*/
/*        ARAnchor anchor = anchorManager.AddComponent<ARAnchor>();
*/        
/*
        if (anchor != null)
        {*/
            // 앵커 위치에 오브젝트 생성
            Instantiate(objectPrefab, anchorPose.position, anchorPose.rotation);
            /*Debug.Log("앵커가 생성되었습니다: " + anchor.transform.position);*/
        //}
       
    }
}
