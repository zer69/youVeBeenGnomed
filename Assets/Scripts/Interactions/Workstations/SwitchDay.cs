using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SwitchDay : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject sleepText;
    [SerializeField] private GameStateManager currentDay;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private b_GameEvent switchDay;
    [SerializeField] private s_GameEvent hint;

    private float fadeAmount;

    private Color screenColor;

    public bool Interact(Interactor interactor)
    {
        StartNewDay();
        return true;
    }

    void StartNewDay()
    {
        StartCoroutine(Sleeping());
        switchDay.Raise(true);
    }

    IEnumerator Sleeping()
    {
        playerInput.DeactivateInput();
        blackScreen.SetActive(true);
        screenColor = blackScreen.GetComponent<Image>().color;
        sleepText.SetActive(true);

        while(blackScreen.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = screenColor.a + (fadeSpeed * Time.deltaTime);

            screenColor = new Color(screenColor.r, screenColor.g, screenColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = screenColor;
            yield return new WaitForEndOfFrame();
        }

        playerInput.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        yield return new WaitForSeconds(2);
        playerInput.ActivateInput();
        sleepText.SetActive(false);

        while (blackScreen.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = screenColor.a - (fadeSpeed * Time.deltaTime);

            screenColor = new Color(screenColor.r, screenColor.g, screenColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = screenColor;
            yield return new WaitForEndOfFrame();
        }

        hint.Raise("Day " + currentDay.day);
        blackScreen.SetActive(false);
    }
}
