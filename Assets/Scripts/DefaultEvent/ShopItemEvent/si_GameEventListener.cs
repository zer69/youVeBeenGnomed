using UnityEngine;
using UnityEngine.Events;

public class si_GameEventListener : MonoBehaviour
{
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)] public si_GameEvent Event;
    [BackgroundColor(0.75f, 0f, 1.5f, .4f)] public UnityEvent<ShopItem> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(ShopItem obj)
    { Response.Invoke(obj); }
}

