using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class eo_GameEvent : ScriptableObject
{
    private List<eo_GameEventListener> listeners =
         new List<eo_GameEventListener>();

    public void Raise(Order obj, int j)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj, j);
    }

    public void RegisterListener(eo_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(eo_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
