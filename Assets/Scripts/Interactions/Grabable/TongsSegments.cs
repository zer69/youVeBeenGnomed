using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongsSegments : MonoBehaviour
{
    [SerializeField] private Transform segment1;
    [SerializeField] private Transform segment2;
    [SerializeField] private Transform tongsPosition;

    private float rotationValue;
    private float ingotRotationValue = 46.319f;
    private float weaponRotationValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForChildren();
        RenderTongs();
    }

    void CheckForChildren()
    {
        if (tongsPosition.childCount == 0)
        {
            rotationValue = 90f;
            return;
        }
            
        Ingot.CompletionStatus completionstatus = Ingot.CompletionStatus.Raw;
        foreach (Transform child in tongsPosition)
            completionstatus = child.GetComponent<Ingot>().status;

        switch (completionstatus)
        {
            case Ingot.CompletionStatus.Raw:
                rotationValue = ingotRotationValue;
                break;
            case Ingot.CompletionStatus.Melted:
                rotationValue = ingotRotationValue;
                break;
            default:
                rotationValue = weaponRotationValue;
                break;
        }
    }

    void RenderTongs()
    {
        segment1.localRotation = Quaternion.Euler(rotationValue, 90f, 90f);
        segment2.localRotation = Quaternion.Euler(rotationValue, -90f, -90f);
    }
}
