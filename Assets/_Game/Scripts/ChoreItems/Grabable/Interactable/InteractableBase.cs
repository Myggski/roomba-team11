using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class InteractableBase : ChoreItemBase, IFocusable {
    [SerializeField]
    private Material outlineMaterial;
    [SerializeField]
    private Color outlineColor;
    [SerializeField]
    private float outlineWidth;

    private FocusHandler _focusHandler;

    public abstract void Interact();
    public void AddHighlight() {
        _focusHandler.AddHighlight();
    }

    public void RemoveHighlight() {
        _focusHandler.RemoveHighlight();
    }

    protected override void Awake() {
        base.Awake();

        _focusHandler =
            new FocusHandler(
                outlineMaterial, 
                outlineColor, 
                outlineWidth, 
                GetComponent<Renderer>()
            );
    }
}