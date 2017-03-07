using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Guided : FlightGuidance
{
    public float Proportional = 4f;
    public float MaxAcceleration = 50;
    public float MaxLockAngle = 70f;
    public Rigidbody Target;

    protected override void Start()
    {
        base.Start();
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }

    protected virtual void FixedUpdate()
    {
        if (Target == null)
        {
            return;
        }

        // https://en.wikipedia.org/wiki/Proportional_navigation
        var missile = GetComponent<Rigidbody>();
        var range = Target.position - missile.position;

        var lineOfSightAngle = Vector3.Angle(missile.velocity.normalized, range.normalized);

        if (lineOfSightAngle < -MaxLockAngle ||
            lineOfSightAngle > MaxLockAngle)
        {
            Target = null;
            return;
        }

        var relativeVelocity = Target.velocity - missile.velocity;
        var rotationVector = Vector3.Cross(range, relativeVelocity)/Vector3.Dot(range, range);
        var acceleration = -Proportional * Vector3.Cross(
                               relativeVelocity.magnitude * missile.velocity.normalized,
                               rotationVector);

        acceleration = new Vector3(
            Mathf.Clamp(acceleration.x, -MaxAcceleration, MaxAcceleration),
            Mathf.Clamp(acceleration.y, -MaxAcceleration, MaxAcceleration),
            Mathf.Clamp(acceleration.z, -MaxAcceleration, MaxAcceleration));

        missile.AddForce(acceleration * missile.mass);
        missile.rotation = Quaternion.LookRotation(missile.velocity.normalized);
    }
}
