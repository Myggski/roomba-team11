using UnityEngine;

public class FocusHandler  {
    private Material _outlineMaterial;
    private Color _outlineColor;
    private float _outlineWidth;
    
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    private Material _originalSharedMaterial;

    public FocusHandler(Material outlineMaterial, Color outlineColor, float outlineWidth, Renderer renderer) {
        _outlineMaterial = outlineMaterial;
        _outlineColor = outlineColor;
        _outlineWidth = outlineWidth;
        _renderer = renderer;

        _originalSharedMaterial = _renderer.sharedMaterial;
    }

    private void SetMaterialProperties() {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _materialPropertyBlock.SetColor("_OutlineColor", _outlineColor);
        _materialPropertyBlock.SetFloat("_OutlineWidth", _outlineWidth);

        _renderer.sharedMaterial = _outlineMaterial;
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public void AddHighlight() {
        SetMaterialProperties();
    }

    public void RemoveHighlight() {
        _renderer.sharedMaterial = _originalSharedMaterial;
    }
}