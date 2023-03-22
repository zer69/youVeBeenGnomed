using UnityEngine;
using UnityEngine.Events;

public class col_GameEventListener : MonoBehaviour
{
    public col_GameEvent Event;
    public UnityEvent<Collider> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(Collider obj)
    { Response.Invoke(obj); }
}

