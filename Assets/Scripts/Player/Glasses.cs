using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glasses : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int maxEnergy = 10;

    private int currentEnergy;
    private bool glassesOn = false;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = maxEnergy;
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
            StartCoroutine(BlacksmithVision());
            Debug.Log("Glasses activated");
        }
        else if(currentEnergy == 0)
        {
            Debug.Log("Glasses is not charged");
        }
        else
        {
            Debug.Log("Glasses deactivated");
        }
    }

    IEnumerator BlacksmithVision()
    {
        while (currentEnergy > 0 && glassesOn == true)
        {
            yield return new WaitForSeconds(1);
            if (glassesOn)
            {
                currentEnergy -= 1;
                Debug.Log("Current charge of glasses is " + currentEnergy);
            }
        }

        glassesOn = false;
    }
}
