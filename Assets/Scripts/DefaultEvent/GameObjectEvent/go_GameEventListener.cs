using UnityEngine;
using UnityEngine.Events;

public class go_GameEventListener : MonoBehaviour
{
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public go_GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, .4f)] public UnityEvent<GameObject> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(GameObject obj)
    { Response.Invoke(obj); }
}

