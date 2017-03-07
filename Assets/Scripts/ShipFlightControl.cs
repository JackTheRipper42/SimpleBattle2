using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class ShipFlightControl : MonoBehaviour
{
    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
        Rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
                                RigidbodyConstraints.FreezeRotationX |
                                RigidbodyConstraints.FreezeRotationZ;
        // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
    }

    protected Rigidbody Rigidbody { get; private set; }
}

