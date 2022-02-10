using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineHighlight : MonoBehaviour {
    [SerializeField]
    private List<HighlightInformation> outlineInformationList = new List<HighlightInformation>();
    [SerializeField]
    private Color outlineColor = Color.black;
    [SerializeField]
    [Range(0, 10)]
    private float outlineWidth = 1;
    [SerializeField]
    private bool useConsistentWidth = true;
    
    private MaterialPropertyBlock _materialPropertyBlock;

    public void AddHighlight() {
        outlineInformationList.ForEach(rendererToHighlight => rendererToHighlight.SetOutline());
        SetMaterialProperties();
    }

    public void RemoveHighlight() {
        outlineInformationList.ForEach(rendererToHighlight => rendererToHighlight.Reset());
    }

    private void Setup() {
        outlineInformationList.ForEach(rendererToHighlight => rendererToHighlight.Setup());
        SetMaterialProperties();
    }

    /// <summary>
    /// Set material values in you play and is in editor-mode
    /// </summary>
    private void TrySetMaterialProperties() {
        if (Application.isEditor && Application.isPlaying) {
            SetMaterialProperties();
        }
    }
    
    /// <summary>
    /// Updates the material-properties
    /// </summary>
    private void SetMaterialProperties() {
        _materialPropertyBlock = new MaterialPropertyBlock();

        _materialPropertyBlock.SetColor("_OutlineColor", outlineColor);
        _materialPropertyBlock.SetFloat("_OutlineWidth", outlineWidth);
        _materialPropertyBlock.SetFloat("_UseConsistentWidth", useConsistentWidth ? 1 : 0);
        
        outlineInformationList.ForEach(rendererToHighlight => rendererToHighlight.SetMaterialProperties(_materialPropertyBlock));
    }

    private void Awake() => Setup();
    private void OnValidate() => TrySetMaterialProperties();
    
    [Serializable]
    private class HighlightInformation {
        [Tooltip("What mesh to highlight")]
        [SerializeField]
        private Renderer _renderer;
        [Tooltip("What outline material it should highlight with")]
        [SerializeField]
        public Material _outlineMaterial;

        private Material[] _originalMaterials;

        public void SetOutline() {
            Material[] sharedMaterials = _renderer.sharedMaterials;

            if (sharedMaterials.Length > 1) {
                sharedMaterials[0] = _outlineMaterial;
            }

            _renderer.sharedMaterials = sharedMaterials;
        }

        public void SetMaterialProperties(MaterialPropertyBlock materialPropertyBlock) {
            _renderer.SetPropertyBlock(materialPropertyBlock);
        }

        public void Setup() {
            _originalMaterials = _renderer.sharedMaterials;

            if (_originalMaterials.Length < 2) {
                _originalMaterials = _originalMaterials
                    .Concat(_originalMaterials)
                    .ToArray();
            }

            _renderer.sharedMaterials = _originalMaterials;
        }

        public void Reset() {
            _renderer.sharedMaterials = _originalMaterials;
        }
    }
}
