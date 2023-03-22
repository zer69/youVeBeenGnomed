using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glasses : MonoBehaviour
{
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [Header("No edit")]

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private b_GameEvent activateGlasses;
    [SerializeField] private b_GameEvent deactivateGlasses;
    [SerializeField] private i_GameEvent decreasedEnergy;
    [SerializeField] private i_GameEvent setEnergyMaxValue;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Energy parameters")]

    [SerializeField] private int maxEnergy = 10;
    [SerializeField] private float dischargingSpeed = 1f;

    

    private int currentEnergy;
    private bool glassesOn = false;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
        setEnergyMaxValue.Raise(maxEnergy);
        playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Glasses":

                if (context.phase == InputActionPhase.Started)
                {
                    glassesOn = !glassesOn;
                    ActivateGlasses(glassesOn);
                }

                break;
        }
    }

    void ActivateGlasses(bool activate)
    {
        if (activate && currentEnergy != 0)
        {
            activateGlasses.Raise(glassesOn);
            StartCoroutine(BlacksmithVision());
            Debug.Log("Glasses activated");
        }
        else if(currentEnergy == 0)
        {
            Debug.Log("Glasses is not charged");
        }
        else
        {
            deactivateGlasses.Raise(true);
            Debug.Log("Glasses deactivated");
        }
    }

    IEnumerator BlacksmithVision()
    {
        while (currentEnergy > 0 && glassesOn == true)
        {
            yield return new WaitForSeconds(dischargingSpeed);
            if (glassesOn)
            {
                currentEnergy -= 1;
                decreasedEnergy.Raise(currentEnergy);
                Debug.Log("Current charge of glasses is " + currentEnergy);
            }
        }

        deactivateGlasses.Raise(true);
        glassesOn = false;

        if (currentEnergy == 0)
        {
            Debug.Log("Glasses discharged");
        }        
    }
}
