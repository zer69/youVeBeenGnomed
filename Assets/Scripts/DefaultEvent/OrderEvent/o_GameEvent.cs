using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class o_GameEvent : ScriptableObject
{
    private List<o_GameEventListener> listeners =
        new List<o_GameEventListener>();

    public void Raise(Order obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(o_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(o_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}


