using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event listener that can be added to a gameObject
/// Calls the connected methods when the event triggers
/// </summary>
public class GameEventChoreItemSetListener : MonoBehaviour
{
    [Tooltip("Event that can be triggered")]
    [SerializeField]
    private GameEventChoreItemSet Event;

    [Tooltip("The Response when the event is triggered")]
    [SerializeField]
    private UnityEvent<RuntimeSetBase> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventCalled(RuntimeSetBase value)
    {
        Response.Invoke(value);
    }
}