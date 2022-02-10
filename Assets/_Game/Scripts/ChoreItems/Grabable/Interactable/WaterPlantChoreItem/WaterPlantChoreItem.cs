using System.Collections;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WaterPlantChoreItem : InteractableBase {
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float animationStartDelay = 0.5f;
    [SerializeField]
    private WaterPlantChoreItemSet waterPlantChoreItemSet;
    [SerializeField]
    private GameEventChoreItemSet onChoreItemCompleted;
    
    [SerializeField] 
    private string _wateringSound;
    public override void Interact() {
        StartCoroutine(StartAnimation());
        
        ChoreCompleted = true;
        onChoreItemCompleted?.Call(waterPlantChoreItemSet);
    }

    private IEnumerator StartAnimation() {
        FMOD.Studio.EventInstance wateringSoundEvent = Sounds.CreateSoundEvent(_wateringSound, transform);
        Sounds.PlaySound(wateringSoundEvent);
        yield return new WaitForSeconds(animationStartDelay);
        animator.SetTrigger("Watered");
        //Sounds.StopSound(wateringSoundEvent, STOP_MODE.ALLOWFADEOUT);
    }
    
    protected override void Awake() {
        base.Awake();
        
        waterPlantChoreItemSet.Add(this);
    }

    private void OnDestroy() => waterPlantChoreItemSet.Remove(InstanceId);
}
