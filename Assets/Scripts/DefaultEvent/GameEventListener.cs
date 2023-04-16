using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, 0.4f)] public UnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    { Response.Invoke(); }
}
