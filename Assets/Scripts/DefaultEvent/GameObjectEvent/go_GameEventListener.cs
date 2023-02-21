using UnityEngine;
using UnityEngine.Events;

public class go_GameEventListener : MonoBehaviour
{
    public go_GameEvent Event;
    public UnityEvent<GameObject> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(GameObject obj)
    { Response.Invoke(obj); }
}

