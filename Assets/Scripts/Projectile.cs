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
        var health = collision.collider.GetComponentInParent<Health>();
        if (health != null)
        {
            health.Hit(Damage);
            Destroy(gameObject);
        }
    }
}
