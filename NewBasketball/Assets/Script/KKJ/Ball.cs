using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Ball : MonoBehaviour
{
    [SerializeField]
    [Header("농구공 프리팹")]
    private GameObject ballPrefab;

    [SerializeField]
    [Header("볼 발사 위치(생성 위치)")]
    private Transform shootPoint;

    [SerializeField]
    [Header("최대 힘")]
    private float maxForce = 20.0f;

    [SerializeField]
    [Header("최대 회전 힘")]
    private float maxSpin = 10f;

    [SerializeField]
    [Header("풀의 크기")]
    private int poolSize = 10;

    private Queue<GameObject> ballPool = new Queue<GameObject>(); //오브젝트 풀
    private GameObject currentBall;  // 현재 공

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private Vector2 startMousePosition;
    private Vector2 endMousePosition;

    private bool isCreateBall = false; //공 생성 플래그 (공이 생성되어있을 때만 스와이프 처리)

    [SerializeField]
    [Tooltip("화면 스와이프 가이드 이미지")]
    private GameObject swipeGuide;

    void Start()
    {
        InitializePool(); // 오브젝트 풀 초기화
        CreateNewBall();  // 첫번째 공 생성
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(isCreateBall)
        {
            // PC 마우스 입력 처리
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endMousePosition = Input.mousePosition;
                Vector2 swipeDirection = endMousePosition - startMousePosition;
                ShootBallWithSwipe(swipeDirection);

                // 마우스 버튼을 뗐을 때
                // 골 성공 여부 확인 코루틴 시작
                StartCoroutine(CheckGoalAndUseChance());
            }

            // 손가락 터치 입력 처리 (모바일)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) // 터치상태가 터치 시작 했을 때
                {
                    startTouchPosition = touch.position; //터치 위치를 터치 시작 위치로 지정
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

        if (Goal.Instance.isGoal) //골을 넣었다면 기회 차감 X
        {
            Goal.Instance.ResetGoal(); // 골 상태 리셋
        }
        else
        {
            ChanceManager.Instance.UseChance(); //기회 차감
            if (ChanceManager.Instance.currentChance <= 0)
            {
                ChanceManager.Instance.GameOver(); //게임 오버
            }
            else
            {
                Goal.Instance.ResetGoal(); //골 상태 리셋
            }
        }
    }

    // 오브젝트 풀 초기화
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.SetActive(false); // 프리팹을 비활성화 상태로 생성
            ballPool.Enqueue(ball); // 공을 Pool에 보관
        }
    }

    // 오브젝트 풀에서 공 가져오기
    private GameObject GetBallFromPool()
    {
        if (ballPool.Count > 0)
        {
            GameObject ball = ballPool.Dequeue();
            ball.SetActive(true); //공을 가져오면서 활성화
            return ball;
        }
        else
        {
            Debug.LogWarning("풀에 공이 부족. 새로 생성");
            return Instantiate(ballPrefab);
        }
    }

    // 던진 공을 풀에 반환
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


    // 가져온 공 위치 지정 (새로운 공 준비)
    private void CreateNewBall()
    {
        currentBall = GetBallFromPool(); //ball 받아오기
        Rigidbody ballRigid = currentBall.GetComponent<Rigidbody>();
        ballRigid.useGravity = false;
        currentBall.transform.position = shootPoint.position;
        currentBall.transform.rotation = Quaternion.identity;
        isCreateBall = true;
    }



    // 스와이프 방향으로 공 발사 
    private void ShootBallWithSwipe(Vector2 swipeDirection)
    {
        if (currentBall == null) return; // 공이 없으면 리턴

        if (swipeGuide != null && swipeGuide.activeSelf)       //스와이프 가이드가 켜져있다면
        {
            swipeGuide.SetActive(false); //스와이프 가이드 끄기
        }

        Goal.Instance.ResetGoal(); // 공을 던지기 전 골 상태 초기화

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
        StartCoroutine(DeactivateBallAfterTime(currentBall, 3f)); //현재 공과 딜레이시간 전달
        currentBall = null;
        StartCoroutine(WaitAndCreateNewBall()); // 1.0f 기다린 다음 새로운 공 생성

    }

    // 일정 시간 후 공 비활성화
    private IEnumerator DeactivateBallAfterTime(GameObject ball, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBallToPool(ball);
    }

    // 새로운 공 생성을 위한 대기 시간
    private IEnumerator WaitAndCreateNewBall()
    {
        yield return new WaitForSeconds(3.0f);
        CreateNewBall();
    }
}
