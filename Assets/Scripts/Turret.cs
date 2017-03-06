using UnityEngine;

public abstract class Turret : Subsystem
{
    public float ReleadTime = 2f;
    public GameObject ProjectilePrefab;

    private float _lastShot;

    protected override void Start()
    {
        base.Start();
        _lastShot = float.MinValue;
        Ship = GetComponentInParent<Ship>();
        ReferenceData = ProjectilePrefab.GetComponent<FlightGuidance>();
    }

    protected virtual void Update()
    {
        if (Time.time < _lastShot + ReleadTime)
        {
            return;
        }
        CalculateShot();
    }

    protected abstract void CalculateShot();

    protected GameObject Fire(Vector3 aimPoint)
    {
        _lastShot = Time.time;
        var projectile = Instantiate(ProjectilePrefab);
        projectile.transform.position = transform.position;
        projectile.transform.rotation = Quaternion.LookRotation(aimPoint - transform.position);

        foreach (var shipCollider in Ship.GetComponentsInChildren<Collider>())
        {
            foreach (var projectileCollider in projectile.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(projectileCollider, shipCollider);
            }            
        }

        return projectile;
    }

    protected FlightGuidance ReferenceData { get; private set; }

    protected Ship Ship { get; private set; }
}
