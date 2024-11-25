using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGoalPost : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // 골대 움직임 속도
    [SerializeField] private float range = 3f; // 움직임 범위
    private Vector3 startPosition;  //골대 시작 위치

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
