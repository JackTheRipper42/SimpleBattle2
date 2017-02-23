using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Warhead))]
public class ImpactFuse : Fuse
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        var subsystem = collision.collider.GetComponentInParent<Subsystem>();

        if (subsystem != null)
        {
            if (subsystem.IsAlive)
            {
                GetComponent<Warhead>().Hit(subsystem);
                Destroy(gameObject);
            }
            else
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<CapsuleCollider>());
            }
        }
    }
}
