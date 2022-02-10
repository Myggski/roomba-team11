using UnityEngine;

public interface IGrabable {
    public Transform Transform { get; }
    public Rigidbody Rigidbody { get; }
    public BoxCollider BoxCollider { get; }
    public Quaternion OriginalRotation { get; }
    public bool IsBeingHold { get; set; }
}