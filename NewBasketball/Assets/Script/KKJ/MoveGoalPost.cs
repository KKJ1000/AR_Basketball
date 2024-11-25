using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGoalPost : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // ��� ������ �ӵ�
    [SerializeField] private float range = 3f; // ������ ����
    private Vector3 startPosition;  //��� ���� ��ġ

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, range) - range / 2;
        transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
    }
}
