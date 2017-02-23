using UnityEngine;

[DisallowMultipleComponent]
public abstract class FlightGuidance : MonoBehaviour
{
    public float Speed = 4f;
    public float MaxFlightTime = 20f;

    private float _startTime;

    protected virtual void Start()
    {
        _startTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time > _startTime + MaxFlightTime)
        {
            Destroy(gameObject);
        }
    }
}
