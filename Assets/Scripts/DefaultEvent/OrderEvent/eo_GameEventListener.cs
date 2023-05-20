using UnityEngine;
using UnityEngine.Events;

public class eo_GameEventListener : MonoBehaviour
{

    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public eo_GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, .4f)] public UnityEvent<Order, int> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(Order obj, int i)
    { Response.Invoke(obj, i); }
}
