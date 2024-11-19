using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBall : MonoBehaviour
{
    private Vector2 touchBeganPos;      //��ġ ���� ��ġ ����
    private Vector2 touchEndedPos;      //��ġ ���� ��ġ ����
    private Vector2 touchDif;

    public Rigidbody ballRigid;         //�󱸰� ������ٵ�
    public GameObject basketballPrefab; //�󱸰� ������Ʈ
      
    void Start()
    {
        GameObject basketball = Instantiate(basketballPrefab, transform);
    }

    void Update()
    {
        Swipe();
    }

    // ȭ�� ��ġ
    public void Swipe()
    {
        if (Input.touchCount > 0)    //��ġ Ƚ���� 0���� ũ�� (��ġ ���� ��)             
        {
            Touch touch = Input.GetTouch(0); //��ó�� ��ġ�� ��ġ�� ����
            //Touch.phase : ���� �ֱ� �����ӿ��� �հ����� ���� ������ ��Ÿ����.

            if (touch.phase == TouchPhase.Began) //Began : ��ġ�� ���۵Ǿ��� ��
            {
                touchBeganPos = touch.position;  // ��ġ ���� �� ��ġ(position) ����
            }
            if (touch.phase == TouchPhase.Ended) //Ended : �հ����� �������� �� (��ġ�� ������ ��)
            {
                touchEndedPos = touch.position;  // ��ġ ���� �� ��ġ(position) ����
                touchDif = (touchEndedPos - touchBeganPos); // �������� �Ÿ�
                MoveBasketball();
            }
        }
    }

    // �켱 �������� �� ��ŭ �󱸰� ������Ʈ�� Z�� �������� �����̰�
    void MoveBasketball()
    {
        Vector3 dir = Vector3.forward;
        ballRigid.AddForce(dir * touchDif);
    }
    
}
