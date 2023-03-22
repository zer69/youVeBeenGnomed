using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class col_GameEvent : ScriptableObject
{
    private List<col_GameEventListener> listeners =
        new List<col_GameEventListener>();

    public void Raise(Collider obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(col_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(col_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
