using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotation : MonoBehaviour
{
    private GameObject currentCam;
    // Start is called before the first frame update
    void Start()
    {
        currentCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(currentCam.transform);
        transform.rotation = Quaternion.LookRotation(currentCam.transform.forward);
    }

    public void SetCamera(GameObject camera)
    {
        currentCam = camera;
        Debug.Log(currentCam);
    }
}
