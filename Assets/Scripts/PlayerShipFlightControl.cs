using UnityEngine;

public class PlayerShipFlightControl : ShipFlightControl
{
    public float Acceleration = 1f;
    public float AngularAcceleration = 1f;
    public float Velocity = 0f;
    public float AngularVelocity = 0f;

    protected virtual void Update()
    {
        var acceleration = 0f;
        var angularAcceleration = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            acceleration += Acceleration;
        }
        if (Input.GetKey(KeyCode.S))
        {
            acceleration -= Acceleration;
        }
        if (Input.GetKey(KeyCode.A))
        {
            angularAcceleration += AngularAcceleration;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angularAcceleration -= AngularAcceleration;
        }

        Velocity += acceleration * Time.deltaTime;
        AngularVelocity -= angularAcceleration * Time.deltaTime;

        Rigidbody.velocity = transform.forward * Velocity;
        Rigidbody.angularVelocity = transform.up * AngularVelocity;
    }
}
