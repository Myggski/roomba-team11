using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is called.
    /// </summary>
    private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();

    /// <summary>
    /// When the gameEvent is called, it triggers all the listeners
    /// </summary>
    public void Call()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventCalled();
        }
    }

    /// <summary>
    /// Adds event listener, that should be called when event is triggered
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    /// Removes event listener, when a gameObject is disabled.
    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}