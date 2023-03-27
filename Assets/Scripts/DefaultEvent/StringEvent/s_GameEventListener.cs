using UnityEngine;
using UnityEngine.Events;

public class s_GameEventListener : MonoBehaviour
{
    public s_GameEvent Event;
    public UnityEvent<string> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(string obj)
    { Response.Invoke(obj); }
}

