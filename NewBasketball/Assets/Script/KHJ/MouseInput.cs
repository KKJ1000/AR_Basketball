using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private Vector3 startMousePosition;
    private Vector3 endMousePosition;
    private float swipePower;

    public BasketBall basketball;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endMousePosition = Input.mousePosition;

            Vector3 swipeVector = endMousePosition - startMousePosition;
            swipePower = swipeVector.magnitude;

            ShootBall(swipeVector.normalized, swipePower);
        }
    }

    void ShootBall(Vector3 swipeDirection, float power)
    {
        Vector3 worldDirection = new Vector3(swipeDirection.x, 1, swipeDirection.y).normalized;

        basketball.Shoot(worldDirection, power);
    }
}
