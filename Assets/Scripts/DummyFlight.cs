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

    protected virtual void Start()
    {
        _waypointIndex = 0;
        _lastError = new Vector3();
        _sumError = new Vector3();
    }

    protected virtual void FixedUpdate()
    {
        if (_waypointIndex >= Waypoints.Length)
        {
            return;
        }

        var nextWayPoint = Waypoints[_waypointIndex];

        var deltaX = nextWayPoint.position - Rigidbody.position;

        if (deltaX.magnitude < WaypointPrecision)
        {
            _waypointIndex++;
            return;
        }

        var currentVelocity = Rigidbody.velocity;
        var desiredVelocity = deltaX.normalized*Velocity;

        var error = desiredVelocity - currentVelocity;
        _sumError += error;
        var acceleration = Kp*error +
                           Ki*Time.fixedDeltaTime*_sumError +
                           Kd*(error - _lastError)/Time.fixedDeltaTime;

        Debug.DrawLine(Rigidbody.position, Rigidbody.position + acceleration, Color.red, Time.fixedDeltaTime);

        acceleration = new Vector3(
            Mathf.Clamp(acceleration.x, -MaxAcceleration, MaxAcceleration),
            Mathf.Clamp(acceleration.y, -MaxAcceleration, MaxAcceleration),
            Mathf.Clamp(acceleration.z, -MaxAcceleration, MaxAcceleration));

        Debug.DrawLine(Rigidbody.position, Rigidbody.position + acceleration, Color.green, Time.fixedDeltaTime);

        Rigidbody.AddForce(acceleration * Rigidbody.mass);
        Rigidbody.rotation = Quaternion.LookRotation(Rigidbody.velocity.normalized);
    }

    protected Rigidbody Rigidbody
    {
        get { return GetComponent<Rigidbody>(); }
    }
}
