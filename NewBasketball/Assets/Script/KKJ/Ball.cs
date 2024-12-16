using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Ball : MonoBehaviour
{
    [SerializeField]
    [Header("�󱸰� ������")]
    private GameObject ballPrefab;

    [SerializeField]
    [Header("�� �߻� ��ġ(���� ��ġ)")]
    private Transform shootPoint;

    [SerializeField]
    [Header("�ִ� ��")]
    private float maxForce = 20.0f;

    [SerializeField]
    [Header("�ִ� ȸ�� ��")]
    private float maxSpin = 10f;

    [SerializeField]
    [Header("Ǯ�� ũ��")]
    private int poolSize = 10;

    private Queue<GameObject> ballPool = new Queue<GameObject>(); //������Ʈ Ǯ
    private GameObject currentBall;  // ���� ��

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private Vector2 startMousePosition;
    private Vector2 endMousePosition;

    private bool isCreateBall = false; //�� ���� �÷��� (���� �����Ǿ����� ���� �������� ó��)

    [SerializeField]
    [Tooltip("ȭ�� �������� ���̵� �̹���")]
    private GameObject swipeGuide;

    void Start()
    {
        InitializePool(); // ������Ʈ Ǯ �ʱ�ȭ
        CreateNewBall();  // ù��° �� ����
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(isCreateBall)
        {
            // PC ���콺 �Է� ó��
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endMousePosition = Input.mousePosition;
                Vector2 swipeDirection = endMousePosition - startMousePosition;
                ShootBallWithSwipe(swipeDirection);

                // ���콺 ��ư�� ���� ��
                // �� ���� ���� Ȯ�� �ڷ�ƾ ����
                StartCoroutine(CheckGoalAndUseChance());
            }

            // �հ��� ��ġ �Է� ó�� (�����)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) // ��ġ���°� ��ġ ���� ���� ��
                {
                    startTouchPosition = touch.position; //��ġ ��ġ�� ��ġ ���� ��ġ�� ����
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    endTouchPosition = touch.position;
                    Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                    ShootBallWithSwipe(swipeDirection);
                }
            }
        }
    }

    private IEnumerator CheckGoalAndUseChance()
    {
        yield return new WaitForSeconds(2.5f);

        if (Goal.Instance.isGoal) //���� �־��ٸ� ��ȸ ���� X
        {
            Goal.Instance.ResetGoal(); // �� ���� ����
        }
        else
        {
            ChanceManager.Instance.UseChance(); //��ȸ ����
            if (ChanceManager.Instance.currentChance <= 0)
            {
                ChanceManager.Instance.GameOver(); //���� ����
            }
            else
            {
                Goal.Instance.ResetGoal(); //�� ���� ����
            }
        }
    }

    // ������Ʈ Ǯ �ʱ�ȭ
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false); // �������� ��Ȱ��ȭ ���·� ����
            ballPool.Enqueue(ball); // ���� Pool�� ����
        }
    }

    // ������Ʈ Ǯ���� �� ��������
    private GameObject GetBallFromPool()
    {
        if (ballPool.Count > 0)
        {
            GameObject ball = ballPool.Dequeue();
            ball.SetActive(true); //���� �������鼭 Ȱ��ȭ
            return ball;
        }
        else
        {
            Debug.LogWarning("Ǯ�� ���� ����. ���� ����");
            return Instantiate(ballPrefab);
        }
    }

    // ���� ���� Ǯ�� ��ȯ
    private void ReturnBallToPool(GameObject ball)
    {
        Rigidbody ballRigid = ball.GetComponent<Rigidbody>();

        if (ballRigid != null)
        {
            ballRigid.velocity = Vector3.zero;
            ballRigid.angularVelocity = Vector3.zero;
        }

        ball.SetActive(false);
        ball.transform.position = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        ballPool.Enqueue(ball);
    }


    // ������ �� ��ġ ���� (���ο� �� �غ�)
    private void CreateNewBall()
    {
        currentBall = GetBallFromPool(); //ball �޾ƿ���
        Rigidbody ballRigid = currentBall.GetComponent<Rigidbody>();
        ballRigid.useGravity = false;
        currentBall.transform.position = shootPoint.position;
        currentBall.transform.rotation = Quaternion.identity;
        isCreateBall = true;
    }



    // �������� �������� �� �߻� 
    private void ShootBallWithSwipe(Vector2 swipeDirection)
    {
        if (currentBall == null) return; // ���� ������ ����

        if (swipeGuide != null && swipeGuide.activeSelf)       //�������� ���̵尡 �����ִٸ�
        {
            swipeGuide.SetActive(false); //�������� ���̵� ����
        }

        Goal.Instance.ResetGoal(); // ���� ������ �� �� ���� �ʱ�ȭ

        float swipeDistance = swipeDirection.magnitude;

        Vector3 vector = new Vector3(swipeDirection.x, swipeDirection.y, swipeDistance).normalized;

        float force = Mathf.Clamp(swipeDistance, 0, maxForce);

        Rigidbody rigid = currentBall.GetComponent<Rigidbody>();
        rigid.useGravity = true;

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        rigid.AddForce(vector * force, ForceMode.Impulse);

        Vector3 spin =
            new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * maxSpin;

        rigid.AddTorque(spin, ForceMode.Impulse);

        isCreateBall = false;
        StartCoroutine(DeactivateBallAfterTime(currentBall, 3f)); //���� ���� �����̽ð� ����
        currentBall = null;
        StartCoroutine(WaitAndCreateNewBall()); // 1.0f ��ٸ� ���� ���ο� �� ����

    }

    // ���� �ð� �� �� ��Ȱ��ȭ
    private IEnumerator DeactivateBallAfterTime(GameObject ball, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBallToPool(ball);
    }

    // ���ο� �� ������ ���� ��� �ð�
    private IEnumerator WaitAndCreateNewBall()
    {
        yield return new WaitForSeconds(3.0f);
        CreateNewBall();
    }
}
