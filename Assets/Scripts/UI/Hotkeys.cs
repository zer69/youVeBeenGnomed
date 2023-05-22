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
                drop.gameObject.SetActive(false);
                help.gameObject.SetActive(true);
                exit.gameObject.SetActive(false);
                glasses.gameObject.SetActive(true);
                runes.gameObject.SetActive(false);
                rotate.gameObject.SetActive(false);
                build.gameObject.SetActive(false);
                break;
            case "menu":
                break;
            case "inHands":
                break;
            case "whetstone":
                break;
            case "build":
                break;
            case "Enchant":
                break;
        }
    }
}
