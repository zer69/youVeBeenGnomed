using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class b_GameEvent : ScriptableObject
{
    private List<b_GameEventListener> listeners =
        new List<b_GameEventListener>();

    public void Raise(bool obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(b_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(b_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
