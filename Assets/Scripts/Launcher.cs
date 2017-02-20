using System.Linq;
using UnityEngine;


public class Launcher : MonoBehaviour
{
    public float ReleadTime = 2f;
    public GameObject MissilePrefab;

    private float _lastShot;
    private Ship _self;
    private Missile _referenceData;

    protected virtual void Start()
    {
        _lastShot = float.MinValue;
        _self = GetComponentInParent<Ship>();
        _referenceData = MissilePrefab.GetComponent<Missile>();
    }

    protected virtual void Update()
    {
        if (Time.time < _lastShot + ReleadTime)
        {
            return;
        }

        var target = FindObjectsOfType<Ship>()
            .Where(ship => ship != _self)
            .Where(ship =>
                (ship.transform.position - transform.position).magnitude <
                _referenceData.MaxFlightTime*_referenceData.Speed)
            .OrderBy(ship => (ship.transform.position - transform.position).magnitude)
            .FirstOrDefault();

        if (target != null)
        {
            _lastShot = Time.time;
            //var obj  = (Component)Instantiate(
            //    MissilePrefab, transform.position,
            //    Quaternion.LookRotation(target.transform.position - transform.position));
            var obj = Instantiate(MissilePrefab);
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            var missile = obj.GetComponent<Missile>();
            missile.Target = target.GetComponentInParent<Rigidbody>();
        }
    }
}
