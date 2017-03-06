using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GunTurret : Turret
{
    protected override void CalculateShot()
    {
        var target = FindObjectsOfType<Ship>()
            .Where(ship => ship != Ship)
            .OrderBy(ship => (ship.transform.position - transform.position).magnitude)
            .FirstOrDefault();

        if (target != null)
        {
            Vector3 aimingPoint;
            float impactTime;
            if (TryCalculatePointOfImpact(target.GetComponent<Rigidbody>(), out aimingPoint, out impactTime))
            {
                var projectile = Fire(aimingPoint);
                var timeFuse = projectile.GetComponent<TimeFuse>();
                if (timeFuse != null)
                {
                    timeFuse.FuseTime = impactTime;
                }
                Debug.DrawLine(transform.position, aimingPoint, Color.red, ReleadTime);
            }
        }
    }

    private bool TryCalculatePointOfImpact(Rigidbody target, out Vector3 targetPosition, out float impactTime)
    {
        impactTime = CalculateImpactTime(target);

        if (impactTime > 0 && impactTime <= ReferenceData.MaxFlightTime)
        {
            targetPosition = target.position + target.velocity*impactTime;
            return true;
        }
        targetPosition = new Vector3();
        return false;
    }

    private float CalculateImpactTime(Rigidbody target)
    {
        var relativePosition = target.position - transform.position;

        var a = target.velocity.sqrMagnitude - ReferenceData.Speed*ReferenceData.Speed;
        var b = 2f*Vector3.Dot(relativePosition, target.velocity);
        var c = relativePosition.sqrMagnitude;

        if (Mathf.Abs(a) < 0.0001f)
        {
            if (Mathf.Abs(b) < 0.0001f)
            {
                return float.NaN;
            }
            return -c/b;
        }

        var p = b/a;
        var q = c/a;

        var rootPart = p*p/4f - q;

        if (rootPart < 0)
        {
            return float.NaN;
        }

        var x1 = -p/2 + Mathf.Sqrt(rootPart);
        var x2 = -p/2 - Mathf.Sqrt(rootPart);

        var minSolution = float.MaxValue;
        if (x1 > 0 && x1 < minSolution)
        {
            minSolution = x1;
        }
        if (x2 > 0 && x1 < minSolution)
        {
            minSolution = x2;
        }

        if (minSolution < float.MaxValue)
        {
            return minSolution;
        }

        return float.NaN;
    }
}
