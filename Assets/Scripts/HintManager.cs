using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    private TextMeshProUGUI hintText;
    private bool canShowHints;
    [Header("Hint Stats")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float fadingTime;

    // Start is called before the first frame update
    void Start()
    {
        canShowHints = true;
        hintText = transform.Find("Hint").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HintFadeout()
    {
        for (float alpha = 1f; alpha > 0f; alpha -= (Time.deltaTime / fadingTime))
        {
            hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, alpha);
            yield return null;
        }
        canShowHints = true;
        hintText.gameObject.SetActive(false);
        yield return null;
    }

    public void RaiseHint(string prompt)
    {
        if (!canShowHints)
            return;
        canShowHints = false;

        hintText.text = prompt;
        hintText.gameObject.SetActive(true);
        StartCoroutine(HintFadeout());
    }
}
