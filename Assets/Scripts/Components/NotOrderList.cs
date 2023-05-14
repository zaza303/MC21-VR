using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class NotOrderList : MonoBehaviour
{
    public CheckDropManager checkDropManager;
    public TrainingDropManager trainingropManager;
    [SerializeField]
    public bool notReady;

    protected bool isTraining;

    public bool IsStartedScript = false;

    public virtual void Start()
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
            if (item != null)
            {
                item.GetComponent<Collider>().enabled = active;
                item.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = active;
            }
        }
    }

    public void SetColliderActive(bool active)
    {
        foreach (var item in NextDropZones)
        {
            if (item != null)
                item.GetComponent<Collider>().enabled = active;
        }
    }
}
