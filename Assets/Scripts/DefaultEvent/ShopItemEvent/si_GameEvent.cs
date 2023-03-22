using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class si_GameEvent : ScriptableObject
{
    private List<si_GameEventListener> listeners =
        new List<si_GameEventListener>();

    public void Raise(ShopItem obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }

    public void RegisterListener(si_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(si_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
