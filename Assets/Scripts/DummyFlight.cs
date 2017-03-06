using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DummyFlight : MonoBehaviour
{
    public float Velocity = 2f;
    public float MaxAcceleration = 3f;
    public float Kp = 5f;
    public float Ki = 0.2f;
    public float Kd = 0.1f;
    public float WaypointPrecision = 0.5f;
    public Transform[] Waypoints;

    private int _waypointIndex;
    private Vector3 _lastError;
    private Vector3 _sumError;
    private Rigidbody _rigidbody;

    protected virtual void Start()
    {
        _waypointIndex = 0;
        _lastError = new Vector3();
        _sumError = new Vector3();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        if (_waypointIndex >= Waypoints.Length)
        {
            return;
        }

        var nextWayPoint = Waypoints[_waypointIndex];

        var deltaX = nextWayPoint.position - _rigidbody.position;

        if (deltaX.magnitude < WaypointPrecision)
        {
            _waypointIndex++;
            return;
        }

        var currentVelocity = _rigidbody.velocity;
        var desiredVelocity = deltaX.normalized*Velocity;

        var error = desiredVelocity - currentVelocity;
        _sumError += error;
        var acceleration = Kp*error +
                           Ki*Time.fixedDeltaTime*_sumError +
                           Kd*(error - _lastError)/Time.fixedDeltaTime;

        Debug.DrawLine(_rigidbody.position, _rigidbody.position + acceleration, Color.red, Time.fixedDeltaTime);

        acceleration = new Vector3(
            Mathf.Clamp(acceleration.x, -MaxAcceleration, MaxAcceleration),
            Mathf.Clamp(acceleration.y, -MaxAcceleration, MaxAcceleration),
            Mathf.Clamp(acceleration.z, -MaxAcceleration, MaxAcceleration));

        Debug.DrawLine(_rigidbody.position, _rigidbody.position + acceleration, Color.green, Time.fixedDeltaTime);

        _rigidbody.AddForce(acceleration * _rigidbody.mass);
        if (_rigidbody.velocity.sqrMagnitude > 0f)
        {
            _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity.normalized);
        }
    }
}
