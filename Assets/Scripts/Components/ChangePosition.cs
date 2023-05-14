using System.Collections;
using UnityEngine;
using VRTK;

public class ChangePosition : DraggedObject
{
    public override void Awake()
    {
        Done = Close = false;
    }

    public override void Start()
    {
        StartCoroutine(CheckTranslate());
    }

    IEnumerator CheckTranslate()
    {
        if (trainingDropManager.gameObject.activeSelf)
            StartCoroutine(trainingDropManager.Translate());
        else
            StartCoroutine(checkDropManager.Translate());

        yield return new WaitForSeconds(1.3f);

        Done = true;
    }

    private void Update()
    {
        if (Done && !Close)
        {
            Close = true;
            if (trainingDropManager.gameObject.activeSelf)
                trainingDropManager.EndActionsWithDraggedObj();
            else
                checkDropManager.EndActionsWithDraggedObj();
        }
    }

    public override void Grabbed(object sender, SnapDropZoneEventArgs e) { }

    public override void UnGrabbed(object sender, SnapDropZoneEventArgs e) { }
}
