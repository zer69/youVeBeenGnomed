using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class i_GameEvent : ScriptableObject
{
    private List<i_GameEventListener> listeners =
        new List<i_GameEventListener>();

    public void Raise(int obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(i_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(i_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
