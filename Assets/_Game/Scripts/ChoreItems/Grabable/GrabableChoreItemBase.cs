using UnityEngine;

[RequireComponent(
    typeof(Rigidbody),
    typeof(BoxcastCommand),
    typeof(Renderer)
)]
public abstract class GrabableChoreItemBase : ChoreItemBase, IGrabable, IFocusable {
    [SerializeField]
    private Material outlineMaterial;
    [SerializeField]
    private Color outlineColor;
    [SerializeField]
    private float outlineWidth;
    
    public Transform Transform => transform;
    public Rigidbody Rigidbody => _rigidbody;
    public BoxCollider BoxCollider => _boxCollider;
    public Quaternion OriginalRotation => _originalRotation;

    private FocusHandler _focusHandler;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private Quaternion _originalRotation;
    
    public abstract bool IsBeingHold { get; set; }

    protected override void Awake() {
        base.Awake();

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