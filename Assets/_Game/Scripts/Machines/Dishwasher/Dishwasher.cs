using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Dishwasher : MachineBase
{
    [Header("Dishwasher Settings")]
    [SerializeField]
    private DishChoreItemSet dishChoreItemSet;
    [SerializeField] 
    private BoxCollider dishwasherDoorCollider;

    [Header("Cabinet Settings")]
    [SerializeField] 
    private Transform cabinet;
    [Tooltip("Space between each item. NOTE: space is twice the number you enter here.")]
    [SerializeField] 
    private float spacing = 0.5f;

    [Header("Sound Settings")]
    [SerializeField]
    private string rattleSound;
    [SerializeField]
    private string openSound;
    [SerializeField]
    private string closeSound;
    [SerializeField]
    private string washingSound;
    [SerializeField] 
    private string pointSound;
    
    private EventInstance _rattleSoundEvent;
    private EventInstance _openSoundEvent;
    private EventInstance _closeSoundEvent;
    private EventInstance _washingSoundEvent;
    private EventInstance _pointSoundEvent;

    private Vector3 CabinetPosition => cabinet.transform.position;

    protected override bool IsHoldingChoreItem =>
        playerGrab.LeftHandItem is DishChoreItem || playerGrab.RightHandItem is DishChoreItem;
    
    public void OnDishesDropped(int instanceId) {
        if (isInteractable && !isMachineWorking) {
            DishChoreItem dishesDropped = dishChoreItemSet.Get(instanceId);
            PrepareDishes(dishesDropped);
            
            if (!IsHoldingChoreItem || isMachineWorking) {
                TryRemoveInteractable();
            } 

            if (storedChoreItems.Count >= maxNumberOfChoreItemsCollected && !isMachineWorking) {
                StartCoroutine(StartAnimation());
            }
        }
    }

    private void PrepareDishes(DishChoreItem dishesDropped) {
        dishesDropped.gameObject.SetActive(false);       
        dishesDropped.transform.parent = cabinet.transform;
        dishesDropped.transform.position = new Vector3(CabinetPosition.x + spacing * storedChoreItems.Count, CabinetPosition.y, CabinetPosition.z);
        storedChoreItems.Add(dishesDropped.InstanceId);
        Sounds.PlaySound(_rattleSoundEvent);
    }

    protected override void Setup() {
        _rattleSoundEvent = Sounds.CreateSoundEvent(rattleSound, transform);
        _openSoundEvent = Sounds.CreateSoundEvent(openSound, transform);
        _closeSoundEvent = Sounds.CreateSoundEvent(closeSound, transform);
        _washingSoundEvent = Sounds.CreateSoundEvent(washingSound, transform);
        _pointSoundEvent = Sounds.CreateSoundEvent(pointSound, transform);
    }
    
    private IEnumerator StartAnimation() {
        isMachineWorking = true;
        dishwasherDoorCollider.enabled = false;
        machineAnimationManager.PlayCloseAnimation();
        Sounds.PlaySound(_closeSoundEvent);

        // Wait for next frame to be able to get info of the upcoming animation
        yield return null;

        yield return new WaitForSeconds(machineAnimationManager.GetNextAnimationPlayTime());

        machineAnimationManager.PlayWashingAnimation();
        Sounds.PlaySound(_washingSoundEvent);
        
        yield return new WaitForSeconds(animationTime);

        ResetAnimation();
    }

    private void ResetStoredItems() {
        foreach (int instanceId in storedChoreItems) {
            DishChoreItem cleanDishes = dishChoreItemSet.Get(instanceId);
            dishChoreItemSet.SetToComplete(instanceId);
            cleanDishes.gameObject.SetActive(true);
            cleanDishes.gameObject.GetComponent<Rigidbody>().useGravity = true;
            cleanDishes.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            cleanDishes.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        storedChoreItems.Clear();
    }

    private void ResetAnimation() {
        ResetStoredItems();
        
        machineAnimationManager.FinishedWashingAnimation();
        isMachineWorking = false;

        onChoreItemCompleted?.Call(dishChoreItemSet);
        Sounds.StopSound(_washingSoundEvent, STOP_MODE.ALLOWFADEOUT);
        Sounds.PlaySound(_pointSoundEvent);
        Sounds.PlaySound(_openSoundEvent);
        dishwasherDoorCollider.enabled = true;
        
        TryHighlightMachine();
    }

    private void Start() => Setup();
}
