using System.Collections;
using FMOD.Studio;
using UnityEngine;

public class LaundryMachine : MachineBase
{
    [Header("Laundry Settings")]
    [SerializeField]
    private LaundryChoreItemSet laundryChoreItemSet;

    [Header("Sound Settings")]
    [SerializeField]
    private string dropSound;
    [SerializeField]
    private string runningSound;
    
    private EventInstance _dropSoundEvent;
    private EventInstance _runningSoundEvent;
    
    protected override bool IsHoldingChoreItem =>
        playerGrab.LeftHandItem is LaundryChoreItem || playerGrab.RightHandItem is LaundryChoreItem;

    public void OnChoreItemDropped(int instanceId) {
        if (isInteractable && !isMachineWorking) {
            LaundryChoreItem laundryDropped = laundryChoreItemSet.Get(instanceId);
            Sounds.PlaySound(_dropSoundEvent);
            laundryDropped.gameObject.SetActive(false);
            storedChoreItems.Add(laundryDropped.InstanceId);
            
            if (!IsHoldingChoreItem || isMachineWorking) {
                TryRemoveInteractable();
            } 

            if (storedChoreItems.Count >= maxNumberOfChoreItemsCollected && !isMachineWorking) {
                StartCoroutine(StartAnimation());
            }
        }
    }

    protected override void Setup() {
        _dropSoundEvent = Sounds.CreateSoundEvent(dropSound, transform);
        _runningSoundEvent = Sounds.CreateSoundEvent(runningSound, transform);
    }

    private IEnumerator StartAnimation() {
        isMachineWorking = true;
        machineAnimationManager.PlayCloseAnimation();
        
        // Wait for next frame to be able to get info of the upcoming animation
        yield return null;

        yield return new WaitForSeconds(machineAnimationManager.GetNextAnimationPlayTime());
        
        Sounds.PlaySound(_runningSoundEvent);
        machineAnimationManager.PlayWashingAnimation();

        yield return new WaitForSeconds(animationTime);

        ResetAnimation();
    }
    
    private void ResetStoredItems() {
        foreach (int instanceId in storedChoreItems) {
            laundryChoreItemSet.SetToComplete(instanceId);
        }

        storedChoreItems.Clear();
    }

    private void ResetAnimation() {
        ResetStoredItems();

        isMachineWorking = false;
        machineAnimationManager.FinishedWashingAnimation();
        Sounds.StopSound(_runningSoundEvent, STOP_MODE.ALLOWFADEOUT);
        onChoreItemCompleted?.Call(laundryChoreItemSet);

        TryHighlightMachine();
    }
    

}

