using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class PlayerGrab : MonoBehaviour {
    [SerializeField]
    Transform playerRightArm;
    [SerializeField]
    Transform playerLeftArm;
    [SerializeField]
    private float grabRadius;

    [Header("Grab Events")]
    [SerializeField]
    private GameEvent onPlayerGrabLeft;
    [SerializeField]
    private GameEvent onPlayerDropLeft;
    [SerializeField]
    private GameEvent onPlayerGrabRight;
    [SerializeField]
    private GameEvent onPlayerDropRight;

    private int _dirtLayerMask = 1 << 6;
    private IGrabable _leftHandItem;
    private IGrabable _rightHandItem;
    private List<IGrabable> _grabableItems = new List<IGrabable>();
    private SphereCollider _sphereCollider;

    private bool HoldingInLeft => !ReferenceEquals(_leftHandItem, null) && _leftHandItem != null;
    private bool HoldingInRight => !ReferenceEquals(_rightHandItem, null) && _rightHandItem != null;

    public IGrabable LeftHandItem => _leftHandItem;
    public IGrabable RightHandItem => _rightHandItem;

    [SerializeField] 
    private string _armSound;

    public void OnGrabLeft(InputAction.CallbackContext context) {
        if (context.started) {
            TryGrabDropItem(false);
        }
    }

    public void OnGrabRight(InputAction.CallbackContext context) {
        if (context.started) {
            TryGrabDropItem(true);
        }
    }

    private void TryGrabDropItem(bool rightArm) {
        if (rightArm) {
            if (!HoldingInRight) {
                TryGetGrabItem(out _rightHandItem);

                if (!HoldingInRight && HoldingInLeft) {
                    _rightHandItem = _leftHandItem;
                    _leftHandItem = null;
                    Sounds.PlaySound(Sounds.CreateSoundEvent(_armSound, transform));
                    onPlayerDropLeft?.Call();
                }

                if (HoldingInRight) {
                    HoldItem(_rightHandItem, playerRightArm);
                    onPlayerGrabRight?.Call();
                }
            } else {
                InteractorBase interactor = _rightHandItem as InteractorBase;

                if (ReferenceEquals(interactor, null) || !interactor.IsAbleToInteract) {
                    DropItem(ref _rightHandItem);
                    onPlayerDropRight?.Call();   
                } else {
                    interactor.Interact(rightArm);
                }
            }
        } else {
            if (!HoldingInLeft) {
                TryGetGrabItem(out _leftHandItem);

                if (!HoldingInLeft && HoldingInRight) {
                    _leftHandItem = _rightHandItem;
                    _rightHandItem = null;
                    Sounds.PlaySound(Sounds.CreateSoundEvent(_armSound, transform));
                    onPlayerDropRight?.Call();
                }

                if (HoldingInLeft) {
                    HoldItem(_leftHandItem, playerLeftArm);
                    onPlayerGrabLeft?.Call();
                }
            } else {
                InteractorBase interactor = _leftHandItem as InteractorBase;

                if (ReferenceEquals(interactor, null) || !interactor.IsAbleToInteract) {
                    DropItem(ref _leftHandItem);
                    onPlayerDropLeft?.Call();
                } else {
                    interactor.Interact(rightArm);
                }
            }
        }
    }

    private void HoldItem(IGrabable handItem, Transform parent) {
        handItem.IsBeingHold = true;
        handItem.Transform.parent = parent;
        handItem.Transform.localPosition = Vector3.zero;
        handItem.Transform.localRotation = Quaternion.Euler(-205, -90, -90);
        handItem.Rigidbody.useGravity = false;
        handItem.Rigidbody.isKinematic = true;
        handItem.BoxCollider.enabled = false;

        AdjustHoldingPosition(handItem);
    }

    private void AdjustHoldingPosition(IGrabable handItem) {
        if (handItem is not InteractorBase interactor) {
            return;
        }
        
        Transform handlebarTransform = interactor ? interactor.Handlebar : handItem.Transform;
        handItem.Transform.position += handItem.Transform.position - handlebarTransform.transform.position;
    }

    private void TryGetGrabItem(out IGrabable grabItem) {
        grabItem = null;

        if (_grabableItems.Count > 0) {
            Sounds.PlaySound(Sounds.CreateSoundEvent(_armSound, transform));
            // Sorting the grabable to select the nearest item
            List<IGrabable> orderedSelectable = _grabableItems
                .OrderBy(grabableItem => 
                    Vector3.Distance(transform.position, grabableItem.Transform.position))
                .ToList();

            grabItem = orderedSelectable.First();
            _grabableItems.Remove(grabItem);

            if (grabItem is IFocusable) {
                (grabItem as IFocusable).RemoveHighlight();    
            }
        }
    }

    private void DropItem(ref IGrabable handItem) {
        Sounds.PlaySound(Sounds.CreateSoundEvent(_armSound, transform));
        IGrabable grabableItem = handItem;
        handItem = null;
        
        /*
            IsBeingHold triggers a Game Event that a ChoreItem has been dropped
            The handItem need to be null before the event triggers
        */
        grabableItem.IsBeingHold = false;
        grabableItem.Transform.parent = null;
        grabableItem.Rigidbody.useGravity = true;
        grabableItem.Rigidbody.isKinematic = false;
        grabableItem.BoxCollider.enabled = true;
    }

    private void Setup() {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = grabRadius;
    }

    private void OnTriggerEnter(Collider other) {
        if ((_dirtLayerMask & (1 << other.transform.gameObject.layer)) > 0 && other.CompareTag("Grabable")) {
            IGrabable grabItem = other.GetComponent<IGrabable>();

            if (!_grabableItems.Contains(grabItem) && !grabItem.IsBeingHold) {
                _grabableItems.Add(grabItem);

                if (grabItem is IFocusable) {
                    (grabItem as IFocusable).AddHighlight();   
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if ((_dirtLayerMask & (1 << other.transform.gameObject.layer)) > 0 && other.CompareTag("Grabable")) {
            IGrabable grabItem = other.GetComponent<IGrabable>();

            if (_grabableItems.Contains(grabItem)) {
                _grabableItems.Remove(grabItem);
                
                if (grabItem is IFocusable) {
                    (grabItem as IFocusable).RemoveHighlight();   
                }
            }
        }
    }

    private void Awake() => Setup();
}
