using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GunTurret : MonoBehaviour
{
    public float ReleadTime = 2f;
    public GameObject ProjectilePrefab;

    private float _lastShot;
    private Ship _self;
    private Projectile _referenceData;

    protected virtual void Start()
    {
        _lastShot = float.MinValue;
        _self = GetComponentInParent<Ship>();
        _referenceData = ProjectilePrefab.GetComponent<Projectile>();
    }

    protected virtual void Update()
    {
        if (Time.time < _lastShot + ReleadTime)
        {
            return;
        }

        var target = FindObjectsOfType<Ship>()
            .Where(ship => ship != _self)
            .OrderBy(ship => (ship.transform.position - transform.position).magnitude)
            .FirstOrDefault();

        if (target != null)
        {
            Vector3 aimingPoint;
            if (TryCalculatePointOfImpact(target.GetComponent<Rigidbody>(), out aimingPoint))
            {
                _lastShot = Time.time;
                Instantiate(
                    ProjectilePrefab, transform.position,
                    Quaternion.LookRotation(aimingPoint - transform.position));
                Debug.DrawLine(transform.position, aimingPoint, Color.red, ReleadTime);
            }
        }
    }

    private bool TryCalculatePointOfImpact(Rigidbody target, out Vector3 targetPosition)
    {
        var flightTime = 0f;
        var timeDelta = _referenceData.MaxFlightTime / 10000f;
        var velocity = target.velocity.magnitude;
        var angularVelocity = target.angularVelocity.y;
        var heading = target.rotation.eulerAngles.y;

        targetPosition = target.position;

        do
        {
            flightTime += timeDelta;

            var velocityVector = Vector3.forward*velocity*timeDelta;

            // no exact calculation just an approximation
            var newTargetPosition1 = targetPosition + Quaternion.Euler(0, heading, 0)* velocityVector;
            heading += angularVelocity * timeDelta;
            var newTargetPosition2 = targetPosition + Quaternion.Euler(0, heading, 0)* velocityVector;

            targetPosition = (newTargetPosition1 + newTargetPosition2)*0.5f;

            var distance = (targetPosition - transform.position).magnitude;
            var projectileRangeSquare = _referenceData.Speed*flightTime;

            if (Mathf.Abs(distance - projectileRangeSquare) < 0.1f)
            {
                return true;
            }
        } while (flightTime < _referenceData.MaxFlightTime);
        return false;
    }
}
