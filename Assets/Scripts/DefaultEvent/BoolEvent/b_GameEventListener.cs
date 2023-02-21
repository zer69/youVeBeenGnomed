using UnityEngine;
using UnityEngine.Events;

public class b_GameEventListener : MonoBehaviour
{
    public b_GameEvent Event;
    public UnityEvent<bool> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(bool obj)
    { Response.Invoke(obj); }
}

