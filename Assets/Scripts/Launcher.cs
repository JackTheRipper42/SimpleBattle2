using System.Linq;
using UnityEngine;

public class Launcher : Turret
{
    protected override void CalculateShot()
    {
        var target = FindObjectsOfType<Ship>()
            .Where(ship => ship != Ship)
            .Where(ship =>
                (ship.transform.position - transform.position).magnitude <
                ReferenceData.MaxFlightTime*ReferenceData.Speed)
            .OrderBy(ship => (ship.transform.position - transform.position).magnitude)
            .FirstOrDefault();

        if (target != null)
        {
            var missile = Fire(target.transform.position);

            var guidance = missile.GetComponent<Guided>();
            guidance.Target = target.GetComponentInParent<Rigidbody>();
        }
    }
}
