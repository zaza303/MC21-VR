using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractableObject : MonoBehaviour
{
    public VRTK_SnapDropZone SnapDropZone;
    public bool IsSnapped = false;

    private VRTK_InteractableObject interactableObject;

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        //interactableObject.OnInteractableObjectSnappedToDropZone
    }
}
