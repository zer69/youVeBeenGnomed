using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class s_GameEvent : ScriptableObject
{
    private List<s_GameEventListener> listeners =
        new List<s_GameEventListener>();

    public void Raise(string obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(obj);
    }
    

    public void RegisterListener(s_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(s_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

    public IEnumerator DelaySeconds(string obj, float delaySeconds)
    {

        yield return new WaitForSeconds(delaySeconds);
        Raise(obj);
        yield return null;
    }
}
