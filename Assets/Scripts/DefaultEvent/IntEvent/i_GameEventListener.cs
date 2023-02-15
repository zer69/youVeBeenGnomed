using UnityEngine;
using UnityEngine.Events;

public class i_GameEventListener : MonoBehaviour
{
    public i_GameEvent Event;
    public UnityEvent<int> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(int obj)
    { Response.Invoke(obj); }
}

