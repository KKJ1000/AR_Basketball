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
    private Ball ballScript;

    [SerializeField]
    [Tooltip("���� ���� �� Ȱ��ȭ�Ǵ� ���̵� �ؽ�Ʈ")]
    private Text guideText;

    [SerializeField]
    [Tooltip("�ٴ��� �ν� ��Ų �� Ȱ��ȭ �Ǵ� ���̵� �ؽ�Ʈ")]
    private Text guideText1;

    [SerializeField]
    [Tooltip("ȭ�� �������� ���̵� �̹���")]
    private GameObject swipeGuide;

    private bool isGuideTextShown = false; //���̵� �ؽ�Ʈ�� �̹� �������� üũ�ϴ� �÷���
    private bool isFloorDetected = false;  //�ٴ��� �νĵƴ��� üũ�ϴ� �÷���

    void Start()
    {
        isGuideTextShown = false;
        isFloorDetected = false;
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
        //�ٴ��� �̹� �ѹ� �νĵǾ����� ó������ ����
        if (isFloorDetected) return;

        // ���� ������ ��鸸 ó��
        foreach (ARPlane plane in args.added)
        {
            CreateAnchorAtPlaneCenter(plane);
            
            if (!isGuideTextShown) //���̵� �ؽ�Ʈ�� �̹� ǥ�õ��� �ʾ��� ���� ����
            {
                guideText.gameObject.SetActive(false); //�ٴ��� �ν��ϸ� ���̵� �ؽ�Ʈ ��Ȱ��ȭ

                guideText1.gameObject.SetActive(true);  //�ٴ��� �ν��ϸ� ���̵� �ؽ�Ʈ1 Ȱ��ȭ

                isGuideTextShown = true; //���̵� �ؽ�Ʈ�� ǥ�õǾ��� �� true�� �����Ͽ� �ٽô� �ؽ�Ʈ�� ������ �ʰ� ����.
                StartCoroutine(DisableGuideTextAndShowSwipeGuide());
            }

            // �ٴ��� �νĵǾ���.
            isFloorDetected = true;
        }
    }

    IEnumerator DisableGuideTextAndShowSwipeGuide()
    {
        //���̵� �ؽ�Ʈ1�� Ȱ��ȭ���� �� 2�� �ڿ� ��Ȱ��ȭ ��Ű�� �� 1.5�ʵ� �������� ���̵� Ȱ��ȭ
        yield return new WaitForSeconds(2f);
        guideText1.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        swipeGuide.SetActive(true);
    }

    private void CreateAnchorAtPlaneCenter(ARPlane plane)
    {
        // ����� �߽� ��ġ ��������
        Vector3 planeCenter = plane.center;
        Vector3 createPos = new Vector3(planeCenter.x, planeCenter.y - 2, planeCenter.z + 5);
        Pose anchorPose = new Pose(createPos, Quaternion.identity);

        GameObject instantiatedObject = Instantiate(objectPrefab, anchorPose.position, anchorPose.rotation);

        // �ڽ� ������Ʈ �������� (��: "ChildObjectName"�̶�� �̸��� �ڽ� ������Ʈ)
        Transform childTransform = instantiatedObject.transform.Find("ShootPoint");

        if (childTransform != null)
        {
            Vector3 childWorldPosition = childTransform.position;

            // Ball ��ũ��Ʈ�� shootPoint ������Ʈ
            if (ballScript != null)
            {
                ballScript.UpdateShootPoint(childWorldPosition);
                Debug.Log($"ShootPoint ��ġ�� ������Ʈ�Ǿ����ϴ�: {childWorldPosition}");

                // ù ��° �� ����
                ballScript.CreateNewBall();
            }
        }
        else
        {
            Debug.LogWarning("ShootPoint �ڽ� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
