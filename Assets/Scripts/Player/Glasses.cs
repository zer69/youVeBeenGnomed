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
    [SerializeField] private b_GameEvent chargingStatus;
    [SerializeField] private i_GameEvent changeEnergy;
    [SerializeField] private i_GameEvent setEnergyMaxValue;
    [SerializeField] private i_GameEvent setGlassesLayer;
    [SerializeField] private s_GameEvent hint;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Energy parameters")]

    [SerializeField] private int maxEnergy;
    [SerializeField] private float dischargingSpeed = 1f;

    

    private int currentEnergy;
    private int glassesLayer = 1;
    private bool glassesOn = false;
    private bool wearGlasses = true;

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

                if (context.phase == InputActionPhase.Started && wearGlasses)
                {
                    glassesOn = !glassesOn;
                    ActivateGlasses(glassesOn);
                }

                break;

            case "SwitchGlassesLayer":

                if (context.phase == InputActionPhase.Started && glassesOn)
                {
                    glassesLayer += 1;
                    if (glassesLayer > 5)
                        glassesLayer = 1;
                    setGlassesLayer.Raise(glassesLayer);
                }

                break;
        }
    }

    void ActivateGlasses(bool activate)
    {
        if (activate && currentEnergy != 0)
        {
            activateGlasses.Raise(glassesOn);
            setGlassesLayer.Raise(glassesLayer);
            StartCoroutine(BlacksmithVision());
            changeEnergy.Raise(currentEnergy);
            Debug.Log("Glasses activated");
        }
        else if(currentEnergy == 0)
        {
            Debug.Log("Glasses is not charged");
            hint.Raise("Glasses is not charged");
        }
        else
        {
            deactivateGlasses.Raise(true);
            Debug.Log("Glasses deactivated");
        }
    }

    IEnumerator BlacksmithVision()
    {
        while (currentEnergy > 0 && glassesOn == true && wearGlasses)
        {
            yield return new WaitForSeconds(dischargingSpeed);
            if (glassesOn)
            {
                currentEnergy -= 1;
                changeEnergy.Raise(currentEnergy);
                Debug.Log("Current charge of glasses is " + currentEnergy);
            }
        }

        deactivateGlasses.Raise(true);
        glassesOn = false;

        if (currentEnergy == 0)
        {
            Debug.Log("Glasses discharged");
            hint.Raise("Glasses discharged");
        }        
    }

    public void IncreaseEnergy(int energyIncrement)
    {
        if(currentEnergy < maxEnergy)
        {
            currentEnergy += energyIncrement;
            changeEnergy.Raise(currentEnergy);
        }
        else
        {
            currentEnergy = maxEnergy;
            chargingStatus.Raise(false);
        }
    }

    public void GlassesStatus(bool glassesOnPlayer)
    {
        if (glassesOnPlayer)
        {
            wearGlasses = true;
        }
        else
        {
            wearGlasses = false;
        }
    }    
}
