using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CatAnimationManager : MonoBehaviour {
    
    private Animator _animator;
    private string _currentAnimation = "Sleeping";
    private string _previousAnimation;

    private void Awake() {
       
        _animator = GetComponent<Animator>();
    }
    
    public void PlaySleepAnimation() {
        SwapAnimations("Sleeping");
    }

    public void PlayWalkingAnimation() {
        SwapAnimations("Walking");
    }

    public void SwapAnimations(string animation)
    {
        if (_currentAnimation == animation) return;
        _previousAnimation = _currentAnimation;
        _currentAnimation = animation;
        _animator.SetBool(_previousAnimation, false);
        _animator.SetBool(_currentAnimation, true);
    }
}