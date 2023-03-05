using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class t_GameEvent : ScriptableObject
{
    private List<t_GameEventListener> listeners =
        new List<t_GameEventListener>();

    public void Raise(Transform obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(t_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(t_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
