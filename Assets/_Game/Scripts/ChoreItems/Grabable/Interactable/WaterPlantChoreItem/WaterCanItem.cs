using System.Collections.Generic;
using UnityEngine;

public class WaterCanItem : InteractorBase {
    private bool _isBeingHold;

    private int _numberOfWateredPlants;
    private List<InteractableBase> _plantsToWater = new List<InteractableBase>();

    public override bool IsBeingHold {
        get => _isBeingHold;
        set => _isBeingHold = value;
    }

    public override bool IsAbleToInteract => _plantsToWater.Count > 0;

    public override void Interact(bool rightArm) {
        base.Interact(rightArm);

        if (IsAbleToInteract) {
            _plantsToWater[0].Interact();
            _plantsToWater[0].RemoveHighlight();
            _plantsToWater.RemoveAt(0);
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Plant")) {
            InteractableBase interactablePlant = other.gameObject.GetComponent<InteractableBase>();

            if (IsBeingHold && !ReferenceEquals(interactablePlant, null) && !_plantsToWater.Contains(interactablePlant) && !interactablePlant.ChoreCompleted) {
                interactablePlant.AddHighlight();
                _plantsToWater.Add(interactablePlant);    
            }
        }
    }

    protected override void OnTriggerExit(Collider other) {
        if (other.CompareTag("Plant")) {
            InteractableBase interactablePlant = other.gameObject.GetComponent<InteractableBase>();

            if (!ReferenceEquals(interactablePlant, null) && _plantsToWater.Contains(interactablePlant)) {
                interactablePlant.RemoveHighlight();
                _plantsToWater.Remove(interactablePlant);    
            }
        }
    }
}
