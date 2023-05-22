using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotkeys : MonoBehaviour
{
    public Transform interact;
    public Transform drop;
    public Transform help;
    public Transform exit;
    public Transform glasses;
    public Transform runes;
    public Transform rotate;
    public Transform build;

    private void Start()
    {
        ChangeHotkeyState("menu");
    }

    public void ChangeHotkeyState(string state)
    {
        interact.gameObject.SetActive(false);
        drop.gameObject.SetActive(false);
        help.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        glasses.gameObject.SetActive(false);
        runes.gameObject.SetActive(false);
        rotate.gameObject.SetActive(false);
        build.gameObject.SetActive(false);
        switch (state)
        {
            case "main":
                interact.gameObject.SetActive(true);
                help.gameObject.SetActive(true);
                glasses.gameObject.SetActive(true);
                break;
            case "menu":
                break;
            case "esc":
                exit.gameObject.SetActive(true);
                break;
            case "anvil":
                glasses.gameObject.SetActive(false);
                break;
            case "inHands":
                interact.gameObject.SetActive(true);
                help.gameObject.SetActive(true);
                glasses.gameObject.SetActive(true);
                drop.gameObject.SetActive(true);
                break;
            case "whetstone":
                exit.gameObject.SetActive(true);
                rotate.gameObject.SetActive(true);
                glasses.gameObject.SetActive(true);
                break;
            case "build":
                build.gameObject.SetActive(true);
                glasses.gameObject.SetActive(true);
                exit.gameObject.SetActive(true);
                break;
            case "enchant":
                runes.gameObject.SetActive(true);
                glasses.gameObject.SetActive(true);
                exit.gameObject.SetActive(true);
                break;
        }
    }
}
