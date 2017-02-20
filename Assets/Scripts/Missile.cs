using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : Projectile
{
    public float MaxRotationDelta = 10f;
    public Rigidbody Target;
    
    protected virtual void FixedUpdate()
    {
        if (Target == null)
        {
            return;
        }

        var rigidBody = GetComponent<Rigidbody>();
        var aimpoint = Target.velocity*Time.fixedDeltaTime + Target.position;
        var aimPointDirection = Quaternion.LookRotation(aimpoint - rigidBody.position);
        var newDirection = Quaternion.RotateTowards(
            rigidBody.rotation,
            aimPointDirection,
            MaxRotationDelta);

        rigidBody.velocity = newDirection*Vector3.forward*Speed;
        rigidBody.rotation = newDirection;
    }
}
