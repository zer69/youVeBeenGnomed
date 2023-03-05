using UnityEngine;
using UnityEngine.Events;

public class t_GameEventListener : MonoBehaviour
{
    public t_GameEvent Event;
    public UnityEvent<Transform> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(Transform obj)
    { Response.Invoke(obj); }
}

