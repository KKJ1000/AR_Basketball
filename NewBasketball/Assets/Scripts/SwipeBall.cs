using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBall : MonoBehaviour
{
    private Vector2 touchBeganPos;      //터치 시작 위치 저장
    private Vector2 touchEndedPos;      //터치 종료 위치 저장
    private Vector2 touchDif;

    public Rigidbody ballRigid;         //농구공 리지드바디
    public GameObject basketballPrefab; //농구공 오브젝트
      
    void Start()
    {
        GameObject basketball = Instantiate(basketballPrefab, transform);
    }

    void Update()
    {
        Swipe();
    }

    // 화면 터치
    public void Swipe()
    {
        if (Input.touchCount > 0)    //터치 횟수가 0보다 크면 (터치 했을 때)             
        {
            Touch touch = Input.GetTouch(0); //맨처음 터치된 터치의 정보
            //Touch.phase : 가장 최근 프레임에서 손가락이 취한 동작을 나타낸다.

            if (touch.phase == TouchPhase.Began) //Began : 터치가 시작되었을 때
            {
                touchBeganPos = touch.position;  // 터치 시작 시 위치(position) 저장
            }
            if (touch.phase == TouchPhase.Ended) //Ended : 손가락이 떨어졌을 때 (터치가 끝났을 때)
            {
                touchEndedPos = touch.position;  // 터치 종료 시 위치(position) 저장
                touchDif = (touchEndedPos - touchBeganPos); // 스와이프 거리
                MoveBasketball();
            }
        }
    }

    // 우선 스와이프 한 만큼 농구공 오브젝트가 Z축 방향으로 움직이게
    void MoveBasketball()
    {
        Vector3 dir = Vector3.forward;
        ballRigid.AddForce(dir * touchDif);
    }
    
}
