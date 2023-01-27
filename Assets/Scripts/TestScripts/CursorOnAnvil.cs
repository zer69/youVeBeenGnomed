using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOnAnvil : MonoBehaviour
{
    private GameObject anvil;
    private float anvilRange = 3;
    // Start is called before the first frame update
    void Start()
    {
        anvil = GameObject.Find("Anvil");
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - anvil.transform.position.x) < anvilRange && Mathf.Abs(transform.position.z - anvil.transform.position.z) < anvilRange)
        {
            Cursor.lockState = CursorLockMode.None;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
