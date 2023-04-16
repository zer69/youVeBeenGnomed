using UnityEngine;
using UnityEngine.Events;

public class i_GameEventListener : MonoBehaviour
{
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public i_GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, .4f)] public UnityEvent<int> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(int obj)
    { Response.Invoke(obj); }
}

