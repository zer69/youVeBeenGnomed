using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Help : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private List<GameObject> tips;
    [SerializeField] private GameObject Helper;

    private int tipNumber = 0;
    private bool isHelperActive = false;
    // Start is called before the first frame update
    void Start()
    {
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
            case "Help":

                if (context.phase == InputActionPhase.Started)
                {
                    isHelperActive = !isHelperActive;
                    Helper.gameObject.SetActive(isHelperActive);
                }

                break;

            case "Scroll Help left":

                if (context.phase == InputActionPhase.Started && isHelperActive == true)
                {
                    tips[tipNumber].SetActive(false);
                    tipNumber -= 1;
                    if (tipNumber < 0)
                        tipNumber = tips.Count - 1;
                    tips[tipNumber].SetActive(true);
                }

                break;

            case "Scroll Help right":

                if (context.phase == InputActionPhase.Started && isHelperActive == true)
                {
                    tips[tipNumber].SetActive(false);
                    tipNumber += 1;
                    if (tipNumber > tips.Count - 1)
                        tipNumber = 0;
                    tips[tipNumber].SetActive(true);
                }

                break;
        }
    }
}
