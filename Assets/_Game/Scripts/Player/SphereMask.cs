using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SphereMask : MonoBehaviour {
    private enum SphereAnimationState {
        None,
        Grow,
        Shrink
    }
    
    [Header("Transition Settings")]
    [SerializeField]
    [Tooltip("Total time of the growing animation in seconds")]
    private float enteringAnimationTime = 1;
    [SerializeField]
    [Tooltip("Total time of the shrink animation in seconds")]
    private float exitingAnimationTime = 0.5f;
    
    [Header("Mask Effect Settings")]
    [SerializeField]
    [Range(1f, 250f)]
    private float frequency = 125f;
    [SerializeField]
    [Range(0f, 2f)]
    private float amplitude = 0.5f;
    [SerializeField]
    [Range(0f, 10f)]
    private float speed = 1f;
    [SerializeField]
    [Range(0f, 360f)]
    private float angle = 0f;
    [SerializeField]
    [Range(1f, 5f)]
    [Tooltip("The transition animation needs to run again for the radius to be updated.")]
    private float radius = 1f;
    
    private Transform _transform;
    private Camera _mainCamera;
    private int _layerMask = 1 << 7; // VisualObstacle layer
    private Coroutine _animationCoroutine;
    private SphereAnimationState _currentAnimationState;
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    private float _currentSphereRadius;
    private Quaternion _rotation;

    private Vector3 Position => _transform.position;

    private void Setup() {
        _rotation = transform.rotation;
        _renderer = GetComponentInChildren<Renderer>();
        _mainCamera = Camera.main;
        _transform = transform;
        _currentAnimationState = SphereAnimationState.None;
        _currentSphereRadius = 0;
        
        SetMaterialProperties();
    }

    /// <summary>
    /// Checks if a wall is between the camera and the mask
    /// </summary>
    /// <returns></returns>
    private bool IsWallBlockingView() {
        RaycastHit[] raycastHits = new RaycastHit[1];
        Vector3 direction = (_mainCamera.transform.position - Position).normalized;

        int numberOfHitsVisualObstacle = Physics.RaycastNonAlloc(
            Position,
            direction,
            raycastHits,
            Mathf.Infinity, 
            _layerMask
        );

        return numberOfHitsVisualObstacle > 0 && raycastHits[0].collider.CompareTag("Wall");
    }
    
    /// <summary>
    /// Returns an animation state, if it should grow, shrink or do nothing
    /// </summary>
    /// <param name="wallBlockingView">If a wall is between the camera and the mask</param>
    /// <returns></returns>
    private SphereAnimationState GetAnimationState(bool wallBlockingView) {
        SphereAnimationState animationState = SphereAnimationState.None;

        if (wallBlockingView && _currentSphereRadius < radius) {
            animationState = SphereAnimationState.Grow;
        } else if (!wallBlockingView && _currentSphereRadius > 0) {
            animationState = SphereAnimationState.Shrink;
        }

        return animationState;
    }

    /// <summary>
    /// Checks if a wall is between the camera and the mask and if the mask should animate to grow/shrink
    /// </summary>
    private void TryAnimate() {
        if (Application.isEditor && Application.isPlaying) {
            SetMaterialProperties();
        }
        
        bool isWallBlockingView = IsWallBlockingView();
        SphereAnimationState animationState = GetAnimationState(isWallBlockingView);

        if (animationState == SphereAnimationState.None || animationState == _currentAnimationState) {
            return;
        }
        
        if (!ReferenceEquals(_animationCoroutine, null)) {
            StopCoroutine(_animationCoroutine);    
        }

        _currentAnimationState = animationState;
        _animationCoroutine = StartCoroutine(Animate(_currentSphereRadius, animationState));
    }
    
    /// <summary>
    /// Animates to shrink or to grow depending on the animation-state
    /// </summary>
    /// <param name="sphereRadius">The current radius of the mask</param>
    /// <param name="animationState">The animation it should do</param>
    /// <returns></returns>
    private IEnumerator Animate(float sphereRadius, SphereAnimationState animationState) {
        float time = animationState == SphereAnimationState.Grow 
            ? enteringAnimationTime
            : exitingAnimationTime;
        float scaleToRadius = animationState == SphereAnimationState.Grow
            ? radius 
            : 0;

        while (time > 0f) {
            yield return null;

            time = time - Time.deltaTime >= 0 
                ? time - Time.deltaTime 
                : 0;
            
            _currentSphereRadius = Mathf.Lerp(scaleToRadius, sphereRadius, time / enteringAnimationTime);
            _materialPropertyBlock.SetFloat("_Radius", _currentSphereRadius);
            _renderer.SetPropertyBlock(_materialPropertyBlock);

            // If the animation is interrupted midway, it should break the loop
            if (ReferenceEquals(_animationCoroutine, null)) {
                break;
            }
        }
        
        _currentAnimationState = SphereAnimationState.None;
    }
    
    /// <summary>
    /// This updates the radius when it's being changed in the inspector
    /// </summary>
    private void TryUpdateRadius() {
        if (_renderer == null) {
            return;
        }
        
        if (_currentAnimationState == SphereAnimationState.None && !_currentSphereRadius.Equals(radius)) {
            _currentSphereRadius = radius;

            SetMaterialProperties();
        }
    }

    /// <summary>
    /// Updates the material-properties
    /// </summary>
    private void SetMaterialProperties() {
        _materialPropertyBlock = new MaterialPropertyBlock();

        _materialPropertyBlock.SetFloat("_Frequency", frequency);
        _materialPropertyBlock.SetFloat("_Amplitude", amplitude);
        _materialPropertyBlock.SetFloat("_Speed", speed);
        _materialPropertyBlock.SetFloat("_Angle", angle);
        _materialPropertyBlock.SetFloat("_Radius", _currentSphereRadius);
        
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void Awake() => Setup();
    private void Update() => TryAnimate();
    private void OnValidate() => TryUpdateRadius();
}
