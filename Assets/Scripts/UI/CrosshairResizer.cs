using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairResizer : MonoBehaviour
{
    private bool crosshairState;
    private RectTransform crosshair;

    // Start is called before the first frame update
    void Start()
    {
        crosshairState = false;
        crosshair = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        ResizeCrosshair();
        crosshairState = false;
    }

    private void ResizeCrosshair()
    {
        if (crosshairState)
            crosshair.sizeDelta = new Vector2(20, 20);
        else
            crosshair.sizeDelta = new Vector2(8, 8);
    }
    public void SetState(bool state)
    {
        crosshairState = state;
    }
}
