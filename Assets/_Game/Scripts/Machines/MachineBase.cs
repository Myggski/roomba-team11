using System.Collections.Generic;
using UnityEngine;

public abstract class MachineBase : MonoBehaviour {
    [Header("Machine Settings")]
    [SerializeField] 
    protected int maxNumberOfChoreItemsCollected = 4;
    [SerializeField]
    protected PlayerGrab playerGrab;
    
    [Header("Animation Settings")]
    [SerializeField] 
    protected float animationTime = 10f;
    [SerializeField]
    protected WashAnimationManager machineAnimationManager;
    
    [Header("Events Settings")]
    [SerializeField]
    protected GameEventChoreItemSet onChoreItemCompleted;
    [SerializeField]
    protected GameEvent onMachineInteractable;
    [SerializeField]
    protected GameEvent onMachineNotInteractable;

    protected bool isInteractable = false;
    protected bool isMachineWorking = false;
    protected List<int> storedChoreItems = new List<int>();

    protected abstract bool IsHoldingChoreItem { get; }
    protected abstract void Setup();

    protected void TryRemoveInteractable() {
        if (isInteractable) {
            isInteractable = false;
            onMachineNotInteractable?.Call();
        }
    }

    public void TryHighlightMachine() {
        if (IsHoldingChoreItem && !isMachineWorking) {
            onMachineInteractable?.Call();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")) {
            isInteractable = true;

            TryHighlightMachine();
        }
    }
    
    private void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("Player")) {
            isInteractable = true;

            TryHighlightMachine();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player")) {
            TryRemoveInteractable();
        }    
    }

    private void Start() => Setup();
}