using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour {
    private Animator _animator;

    private void Setup() {
        _animator = GetComponent<Animator>();
    }
    
    public void PlayIdleAnimation() {
        _animator.SetBool("LowBattery", false);
    }

    public void PlayLowBatteryAnimation() {
        _animator.SetBool("LowBattery", true);
    }
    
    public void PlayGrabLeft() {
        _animator.SetBool("GrabLeft", true);
    }
    
    public void PlayGrabRight() {
        _animator.SetBool("GrabRight", true);
    }

    public void PlayDropLeft() {
        _animator.SetBool("GrabLeft", false);
    }
    
    public void PlayDropRight() {
        _animator.SetBool("GrabRight", false);
    }
    
    public void PlayPourLeft() {
        _animator.SetTrigger("PouringLeft");
    }
    
    public void PlayPourRight() {
        _animator.SetTrigger("PouringRight");
    }
    
    public void PlayCelebrating() {
        _animator.SetTrigger("Celebrating");
    }
    
    public void PlayWinning() {
        _animator.SetBool("IsWinning", true);
    }

    private void Awake() => Setup();
}
