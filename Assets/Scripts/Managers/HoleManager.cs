using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HoleManager : MonoBehaviour
{
    [SerializeField]
    private TrainingDropManager trainingDropManager;

    private VRTK_InteractableObject interactableObject;
    private void Start()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        interactableObject.InteractableObjectSnappedToDropZone += Snapped;
    }

    public void Snapped(object sender, InteractableObjectEventArgs e)
    {
        var SnapDropZone = GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone();
        SnapDropZone.ObjectSnappedToDropZone -= trainingDropManager.EnteredSnapDropZone;
        SnapDropZone.ObjectUnsnappedFromDropZone -= trainingDropManager.ExitedSnapDropZone;
    }

    private void OnDestroy()
    {
        object sender = new object();
        SnapDropZoneEventArgs drop = new SnapDropZoneEventArgs();
        drop.snappedObject = gameObject;
        trainingDropManager.EnteredSnapDropZone(sender, drop);
    }
}
