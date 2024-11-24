using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    public Rigidbody basketballRigidbody;
    public Transform hoop;
    public float maxPower = 15f;
    public float minPower = 5f;
    public float verticalMultiplier = 0.5f;

    public void Shoot(Vector3 direction, float power)
    {
        float clampedPower = Mathf.Clamp(power, minPower, maxPower);

        Vector3 toHoop = hoop.position - transform.position;
        Vector3 horizontalDirection = new Vector3(toHoop.x, 0, toHoop.z).normalized;
        Vector3 initialVelocity = horizontalDirection * clampedPower;
        initialVelocity.y = clampedPower * verticalMultiplier;

        basketballRigidbody.velocity = Vector3.zero;
        basketballRigidbody.velocity = initialVelocity;
    }
}
