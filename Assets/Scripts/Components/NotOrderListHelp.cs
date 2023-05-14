using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class NotOrderListHelp : MonoBehaviour
{
    [SerializeField]
    private CheckDropManager checkDropManager;
    [SerializeField]
    private TrainingDropManager trainingropManager;
    [SerializeField]
    public bool notReady;

    private bool isTraining;

    public void Start()
    {
        isTraining = trainingropManager.gameObject.activeSelf;

        foreach (var item in NextDropZones)
        {
            if (isTraining == false)
            {
                item.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += checkDropManager.EnteredSnapDropZone;
                item.GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone += checkDropManager.ExitedSnapDropZone;
            }
            else
            {
                item.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += trainingropManager.EnteredSnapDropZone;
                item.GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone += trainingropManager.ExitedSnapDropZone;
            }
        }

        if (isTraining)
            SetColliderHighlightActive(false);
    }

    public List<GameObject> NextDropZones = new List<GameObject>();
    public List<GameObject> PossibleObjects = new List<GameObject>();

    public void SetColliderHighlightActive(bool active)
    {
        foreach (var item in NextDropZones)
        {
            item.GetComponent<Collider>().enabled = active;
            item.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = active;
        }
    }

    public void SetColliderActive(bool active)
    {
        foreach (var item in NextDropZones)
            item.GetComponent<Collider>().enabled = active;
    }
}
