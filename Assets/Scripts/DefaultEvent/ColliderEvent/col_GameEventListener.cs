using UnityEngine;
using UnityEngine.Events;

public class col_GameEventListener : MonoBehaviour
{
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public col_GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, 0.4f)] public UnityEvent<Collider> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(Collider obj)
    { Response.Invoke(obj); }
}

