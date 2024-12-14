using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AutoAnchorPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab; // ��ġ�� ������Ʈ ������

    private ARAnchorManager anchorManager; // ARAnchorManager
    private ARPlaneManager planeManager;   // ARPlaneManager

    [SerializeField]
    [Tooltip("���� ���� �� Ȱ��ȭ�Ǵ� ���̵� �ؽ�Ʈ")]
    private Text guideText;

    [SerializeField]
    [Tooltip("�ٴ��� �ν� ��Ų �� Ȱ��ȭ �Ǵ� ���̵� �ؽ�Ʈ")]
    private Text guideText1;

    [SerializeField]
    [Tooltip("ȭ�� �������� ���̵� �̹���")]
    private GameObject swipeGuide;

    private float delay = 5.0f; //���̵� �ؽ�Ʈ ��Ȱ��ȭ ������

    void Start()
    {
        guideText.gameObject.SetActive(true);   //ù��° ���̵� �ؽ�Ʈ Ȱ��ȭ
        guideText1.gameObject.SetActive(false); //�ι�° ���̵� �ؽ�Ʈ ��Ȱ��ȭ
        swipeGuide.SetActive(false);            //�������� ���̵� ��Ȱ��ȭ
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
            guideText.gameObject.SetActive(false); //�ٴ��� �ν��ϸ� ���̵� �ؽ�Ʈ ��Ȱ��ȭ
            guideText.gameObject.SetActive(true);  //�ٴ��� �ν��ϸ� ���̵� �ؽ�Ʈ1 Ȱ��ȭ

            StartCoroutine(DisableGuideTextAfterDelay(delay));
        }
    }

    IEnumerator DisableGuideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); //delay ��ŭ ���
        guideText1.gameObject.SetActive(false); //�ι�° ���̵� �ؽ�Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(2f);    //2�� �� �������� ���̵� Ȱ��ȭ
        swipeGuide.SetActive(true);             //�������� ���̵� Ȱ��ȭ
    }

    private void CreateAnchorAtPlaneCenter(ARPlane plane)
    {
        // ����� �߽� ��ġ ��������
        Vector3 planeCenter = plane.center;
        Vector3 createPos = new Vector3(planeCenter.x, planeCenter.y - 2, planeCenter.z + 5);
        Pose anchorPose = new Pose(createPos, Quaternion.identity);

        Instantiate(objectPrefab, anchorPose.position, anchorPose.rotation);
    }
}
