using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongsSegments : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Transform segment1;
    [SerializeField] private Transform segment2;
    [SerializeField] private Transform tongsPosition;

    private float rotationValue;
    private float ingotRotationValue = 46.319f;
    private float weaponRotationValue = 74.946f;

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
        Ingot.WeaponType weaponType = Ingot.WeaponType.None;
        foreach (Transform child in tongsPosition)
        {
            completionstatus = child.GetComponent<Ingot>().status;
            weaponType = child.GetComponent<Ingot>().weaponType;
            Debug.Log(weaponType);
        }
            

        switch(weaponType)
        {
            case Ingot.WeaponType.Axe:
                weaponRotationValue = 80.5f;
                break;
            case Ingot.WeaponType.Sword:
                weaponRotationValue = 74.946f;
                break;
            case Ingot.WeaponType.Dagger:
                weaponRotationValue = 74.946f;
                break;
            case Ingot.WeaponType.Spear:
                weaponRotationValue = 80.54f;
                break;
        }

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
