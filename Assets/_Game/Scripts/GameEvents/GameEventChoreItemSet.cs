using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEventChoreItemSet", menuName = "Game Events/Game Event ChoreItemSet")]
public class GameEventChoreItemSet : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is called.
    /// </summary>
    private readonly List<GameEventChoreItemSetListener> eventListeners = new List<GameEventChoreItemSetListener>();

    /// <summary>
    /// When the gameEvent is called, it triggers all the listeners
    /// </summary>
    public void Call(RuntimeSetBase value)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventCalled(value);
        }
    }

    /// <summary>
    /// Adds event listener, that should be called when event is triggered
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterListener(GameEventChoreItemSetListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    /// Removes event listener, when a gameObject is disabled.
    public void UnregisterListener(GameEventChoreItemSetListener listener) {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}