using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AutoAnchorPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab; // ��ġ�� ������Ʈ ������

    private ARAnchorManager anchorManager; // ARAnchorManager
    private ARPlaneManager planeManager;   // ARPlaneManager

    void Start()
    {
        // ARAnchorManager�� ARPlaneManager ��������
        anchorManager = FindObjectOfType<ARAnchorManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();

        if (anchorManager == null || planeManager == null)
        {
            Debug.LogError("ARAnchorManager �Ǵ� ARPlaneManager�� ã�� �� �����ϴ�.");
        }

        // AR Plane�� �����Ǹ� ȣ��Ǵ� �̺�Ʈ ���
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ����
        if (planeManager != null)
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // ���� ������ ��鸸 ó��
        foreach (ARPlane plane in args.added)
        {
            CreateAnchorAtPlaneCenter(plane);
        }
    }

    private void CreateAnchorAtPlaneCenter(ARPlane plane)
    {
        // ����� �߽� ��ġ ��������
        Vector3 planeCenter = plane.center;
        Vector3 createPos = new Vector3(planeCenter.x,planeCenter.y - 2,planeCenter.z + 5);
        Pose anchorPose = new Pose(createPos, Quaternion.identity);

        // ��Ŀ ����
        /*ARAnchor anchor = anchorManager.AddAnchor(anchorPose);*/
/*        ARAnchor anchor = anchorManager.AddComponent<ARAnchor>();
*/        
/*
        if (anchor != null)
        {*/
            // ��Ŀ ��ġ�� ������Ʈ ����
            Instantiate(objectPrefab, anchorPose.position, anchorPose.rotation);
            /*Debug.Log("��Ŀ�� �����Ǿ����ϴ�: " + anchor.transform.position);*/
        //}
       
    }
}
