using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float Damage = 10f;
    public float Speed = 4f;
    public float MaxFlightTime = 20f;

    private float _startTime;

    protected virtual void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward*Speed;
        _startTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time > _startTime + MaxFlightTime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        foreach (var contactPoint in collision.contacts)
        {
            Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.magenta, 2f);
        }

        var subsystem = collision.collider.GetComponentInParent<Subsystem>();

        if (subsystem != null)
        {
            if (subsystem.IsAlive)
            {
                subsystem.Hit(Damage);
                Destroy(gameObject);
            }
            else
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider>());
            }
        }
    }
}
