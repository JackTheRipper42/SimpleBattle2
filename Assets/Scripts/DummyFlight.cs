using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DummyFlight : MonoBehaviour
{
    public float Velocity = 2f;
    public float AngularVelocity = 1f;

    protected virtual void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * Velocity;
        GetComponent<Rigidbody>().angularVelocity = transform.up * AngularVelocity;
    }
}
