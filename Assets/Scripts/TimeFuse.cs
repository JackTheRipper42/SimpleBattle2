using UnityEngine;

[RequireComponent(typeof(ExplosiveWarhead))]
public class TimeFuse : Fuse
{
    public float FuseTime;

    private float _explosionTime;

    protected virtual void Start()
    {
        _explosionTime = Time.time + FuseTime;
    }

    protected virtual void Update()
    {
        if (Time.time >= _explosionTime && FuseTime > 0f)
        {
            GetComponent<ExplosiveWarhead>().Explode();
            Destroy(gameObject);
        }
    }
}
