using UnityEngine;

[DisallowMultipleComponent]
public class Warhead : MonoBehaviour
{
    public float Damage = 10f;

    public virtual void Hit(Subsystem subsystem)
    {
        subsystem.Hit(Damage);
    }
}
