using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DummyFlight : MonoBehaviour
{
    public float Velocity = 2f;
    public float MaxAngleChange = 5f;
    public Transform[] Waypoints;

    private int _waypointIndex;

    protected virtual void Start()
    {
        //GetComponent<Rigidbody>().velocity = transform.forward * Velocity;
        _waypointIndex = 0;
    }

    protected virtual void FixedUpdate()
    {
        if (_waypointIndex >= Waypoints.Length)
        {
            return;
        }

        var nextWayPoint = Waypoints[_waypointIndex];

        var deltaX = nextWayPoint.position - Rigidbody.position;

        if (deltaX.magnitude < 0.1f)
        {
            _waypointIndex++;
            return;
        }

        var currentRotation = Rigidbody.rotation;
        var desiredRotation = Quaternion.LookRotation(deltaX.normalized);
        var newRotation = Quaternion.RotateTowards(currentRotation, desiredRotation, MaxAngleChange);

        Rigidbody.velocity = newRotation*Vector3.forward*Velocity;
        Rigidbody.rotation = newRotation;
    }

    protected Rigidbody Rigidbody
    {
        get { return GetComponent<Rigidbody>(); }
    }
}
