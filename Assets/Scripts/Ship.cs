using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    public float Acceleration = 1f;
    public float AngularAcceleration = 1f;
    public float Velocity = 0f;
    public float AngularVelocity = 0f;
    public bool IsPlayer = true;

    private Rigidbody _rigidbody;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    protected virtual void Update()
    {
        if (!IsPlayer)
        {
            return;
        }

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

        Velocity += acceleration*Time.deltaTime;
        AngularVelocity -= angularAcceleration*Time.deltaTime;

        _rigidbody.velocity = transform.forward*Velocity;
        _rigidbody.angularVelocity = transform.up*AngularVelocity;

        //_rigidbody.velocity += transform.forward * acceleration * Time.deltaTime;
        //_rigidbody.angularVelocity -= transform.up*angularAcceleration*Time.deltaTime;
    }
}
