using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    //[BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameEvent TeleportUsed;
    [SerializeField] private Transform detectionZone;
    [SerializeField] private s_GameEvent hotkey;





    public string InteractionPrompt => _prompt;
    // Start is called before the first frame update
    void Start()
    {
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
        detectionZone = transform.Find("TeleportStation").Find("Platform").Find("DetectionZone");
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWeapons();
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Abort":
              
            cam.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
            playerInput.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            break;
        }
    }

    public bool Interact(Interactor interactor)
    {
        //playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //playerInput.transform.localRotation = Quaternion.identity;
        Debug.Log("Teleport is used");
        TeleportUsed.Raise();
        cam.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        playerInput.transform.localRotation = Quaternion.identity;
        hotkey.Raise("menu");
        return true;
    }

    public void EndInteraction()
    {
        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);
        playerInput.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void CheckForWeapons()
    {
        if (detectionZone.GetComponent<TeleportPlatform>().colliderList.Count > 0)
        {
            ActivateSender();
        }
        else
        {
            DeactivateSender();
        }

    }

    void ActivateSender()
    {
        // activate button on ui
    }

    void DeactivateSender()
    {
        // deactivate button on ui
    }
   
}
