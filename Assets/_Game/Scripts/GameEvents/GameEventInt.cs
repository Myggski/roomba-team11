using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEventInt", menuName = "Game Events/Game Event Int")]
public class GameEventInt : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is called.
    /// </summary>
    private readonly List<GameEventIntListener> eventListeners = new List<GameEventIntListener>();

    /// <summary>
    /// When the gameEvent is called, it triggers all the listeners
    /// </summary>
    public void Call(int value)
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
    public void RegisterListener(GameEventIntListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    /// Removes event listener, when a gameObject is disabled.
    public void UnregisterListener(GameEventIntListener listener) {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}