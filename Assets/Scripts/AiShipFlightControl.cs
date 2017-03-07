using UnityEngine;

public class AiShipFlightControl : ShipFlightControl
{
    public float Velocity = 2f;
    public float MaxAcceleration = 3f;
    public float Kp = 5f;
    public float Ki = 0.2f;
    public float Kd = 0.1f;
    public float WaypointPrecision = 0.5f;
    public Rigidbody Target;
    public float Radius = 6f;

    private Vector3 _lastError;
    private Vector3 _sumError;

    protected override void Start()
    {
        base.Start();
        _lastError = new Vector3();
        _sumError = new Vector3();
    }

    protected virtual void FixedUpdate()
    {
        var targetRange = Target.position - Rigidbody.position;
        var angleToNormal = Mathf.Asin(Radius/targetRange.magnitude) * Mathf.Rad2Deg;

        if (float.IsNaN(angleToNormal))
        {
            return;
        }

        var targetDirection = Quaternion.LookRotation(targetRange.normalized);
        var vector1 = targetDirection*Quaternion.Euler(0, angleToNormal, 0)*Vector3.forward;
        var vector2 = targetDirection*Quaternion.Euler(0, -angleToNormal, 0)*Vector3.forward;

        var angleDiff1 = Vector3.Angle(transform.forward, vector1);
        var angleDiff2 = Vector3.Angle(transform.forward, vector2);

        var waypointDirection = angleDiff1 < angleDiff2 ? vector1 : vector2;
        var waypointRange = Mathf.Sqrt(targetRange.sqrMagnitude - Radius*Radius);

        var deltaX = waypointDirection.normalized*waypointRange;

        Debug.DrawLine(Rigidbody.position, Rigidbody.position + deltaX, Color.blue, Time.fixedDeltaTime);

        if (deltaX.magnitude < WaypointPrecision)
        {
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
        if (Rigidbody.velocity.sqrMagnitude > 0f)
        {
            Rigidbody.rotation = Quaternion.LookRotation(Rigidbody.velocity.normalized);
        }
    }
}
