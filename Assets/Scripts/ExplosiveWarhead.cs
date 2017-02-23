using UnityEngine;

public class ExplosiveWarhead : Warhead
{
    public float ExplosionDamage = 10f;
    public float ExplosionRange = 6f;
    public LayerMask ShipLayerMask;

    public virtual void Explode()
    {
        foreach (var affectedCollider in Physics.OverlapSphere(transform.position, ExplosionRange))
        {
            var subsystem = affectedCollider.GetComponent<Subsystem>();
            if (subsystem != null)
            {
                var ray = new Ray(transform.position, subsystem.transform.position - transform.position);
                RaycastHit raycastHitInfo;
                if (Physics.Raycast(ray, out raycastHitInfo, ExplosionRange, ShipLayerMask.value))
                {
                    if (raycastHitInfo.collider.GetComponent<Subsystem>() == subsystem)
                    {
                        var distance = (raycastHitInfo.point - transform.position).magnitude;
                        var effectiveDamage = ExplosionDamage*(1 - distance/ExplosionRange);
                        subsystem.Hit(effectiveDamage);
                    }
                }
            }
        }
    }

    public override void Hit(Subsystem subsystem)
    {
        base.Hit(subsystem);
        Explode();
    }
}
