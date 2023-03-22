using UnityEngine;
using UnityEngine.Events;

public class si_GameEventListener : MonoBehaviour
{
    public si_GameEvent Event;
    public UnityEvent<ShopItem> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(ShopItem obj)
    { Response.Invoke(obj); }
}

