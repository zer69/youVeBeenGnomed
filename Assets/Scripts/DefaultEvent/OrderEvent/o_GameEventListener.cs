using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class o_GameEventListener : MonoBehaviour
{
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public o_GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, .4f)] public UnityEvent<List<Order>> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(List<Order> obj)
    { Response.Invoke(obj); }
}
