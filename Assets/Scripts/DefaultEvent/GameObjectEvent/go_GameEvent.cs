using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class go_GameEvent : ScriptableObject
{
    private List<go_GameEventListener> listeners =
        new List<go_GameEventListener>();

    public void Raise(GameObject obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(go_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(go_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
