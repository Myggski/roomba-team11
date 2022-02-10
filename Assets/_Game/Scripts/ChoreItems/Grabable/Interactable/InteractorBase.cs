
using System.Transactions;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody),
    typeof(BoxcastCommand),
    typeof(Renderer)
)]
public abstract class InteractorBase : MonoBehaviour, IGrabable, IFocusable {
    [Header("Water Can Settings")]
    [SerializeField]
    private Transform handlebar;
    [Header("Outline Settings")]
    [SerializeField]
    private Material outlineMaterial;
    [SerializeField]
    private Color outlineColor;
    [SerializeField]
    private float outlineWidth;

    [Header("Interaction Events")]
    [SerializeField]
    protected GameEvent onInteractLeftHand;
    [SerializeField]
    protected GameEvent onInteractRightHand;

    public abstract bool IsAbleToInteract { get; }
    public abstract bool IsBeingHold { get; set; }
    public Transform Handlebar => handlebar ? handlebar : transform;

    public virtual void Interact(bool rightArm) {
        if (!IsAbleToInteract) {
            return;
        }

        if (rightArm) {
            onInteractRightHand?.Call();
        } else {
            onInteractLeftHand?.Call();
        }
    }

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerExit(Collider other);
    public Transform Transform => transform;
    public Rigidbody Rigidbody => _rigidbody;
    public BoxCollider BoxCollider => _boxCollider;
    public Quaternion OriginalRotation => _originalRotation;

    private FocusHandler _focusHandler;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private Quaternion _originalRotation;

    protected void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _originalRotation = Transform.rotation;

        _focusHandler =
            new FocusHandler(
                outlineMaterial, 
                outlineColor, 
                outlineWidth, 
                GetComponent<Renderer>()
            );
    }
    public void AddHighlight() {
        _focusHandler.AddHighlight();
    }

    public void RemoveHighlight() {
        _focusHandler.RemoveHighlight();
    }
}
