using UnityEngine;

public class WashAnimationManager : MonoBehaviour {
    [SerializeField]
    private Animator _animator;

    public float GetNextAnimationPlayTime() {
        AnimatorClipInfo[] clipInfo = _animator.GetNextAnimatorClipInfo(0);

        return clipInfo.Length > 0
            ? _animator.GetNextAnimatorClipInfo(0)[0].clip.length
            : 1f;
    }
    
    public void PlayCloseAnimation() {
        _animator.SetBool("Close", true);
    }

    public void PlayWashingAnimation()
    {
        _animator.SetBool("Washing", true);
    }
    
    public void FinishedWashingAnimation()
    {
        _animator.SetBool("Washing", false);
        _animator.SetBool("Close", false);
    }
    
}