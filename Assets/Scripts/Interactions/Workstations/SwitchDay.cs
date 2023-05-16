using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchDay : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject sleepText;
    [SerializeField] private float fadeSpeed = 1f;

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
    }

    IEnumerator Sleeping()
    {
        screenColor = blackScreen.GetComponent<Image>().color;
        sleepText.SetActive(true);

        while(blackScreen.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = screenColor.a + (fadeSpeed * Time.deltaTime);

            screenColor = new Color(screenColor.r, screenColor.g, screenColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = screenColor;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);
        sleepText.SetActive(false);

        while (blackScreen.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = screenColor.a - (fadeSpeed * Time.deltaTime);

            screenColor = new Color(screenColor.r, screenColor.g, screenColor.b, fadeAmount);
            blackScreen.GetComponent<Image>().color = screenColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
