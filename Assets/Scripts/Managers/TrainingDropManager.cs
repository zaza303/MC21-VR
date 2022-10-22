using UnityEngine;
using System.Collections.Generic;
using VRTK;
using System.Collections;
using System.Linq;
using System;

public class TrainingDropManager : DropManager
{
    public List<GameObject> DropObjOrderList = new List<GameObject>();
    public List<GameObject> tempDroppedObjectsList = new List<GameObject>();

    public List<DropZoneBase> currentObjects = new List<DropZoneBase>();
    public List<DropZoneBase> allCurrentObjects = new List<DropZoneBase>();
    private int curCountOfActions = 0;
    private bool isStarted;
    private int countBeforeAutoSnap = 0;

    public Action OnStateChanged;
    protected override void Awake()
    {
        base.Awake();
        additional = listIsOver = isStarted = false;
    }

    public override void EnteredSnapDropZone(object sender, SnapDropZoneEventArgs e)
    {
        if (isStarted && listIsOver == false)
        {
            if (!additional)
                Check(e.snappedObject);
            else
            {
                tempDroppedObjectsList.Add(e.snappedObject);
                e.snappedObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public override void ExitedSnapDropZone(object sender, SnapDropZoneEventArgs e)
    {
        if (listIsOver == false)
        {
            tempDroppedObjectsList.Remove(e.snappedObject);
        }
    }

    private void Start()
    {
        foreach (var item in DropZones)
        {
            item.SnapDropZone.ObjectSnappedToDropZone += EnteredSnapDropZone;
            item.SnapDropZone.ObjectUnsnappedFromDropZone += ExitedSnapDropZone;
        }

        foreach (var item in DropZones)
            item.SetColliderHighlightActive(false);

        AutoSnappingScript.UpdateButton(false);
    }

    public void StartTraining()
    {
        bool activatedObjects = false;
        if (ListOfNames.Count > 0)
        {
            //Вызов события для смены состояния на ProgressUI
            StateChanged();

            bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
            AutoSnappingScript.UpdateButton(flag);

            isStarted = true;
            var nums = ListOfNames[0].Split(' ');
            foreach (var num in nums)
            {
                DropZoneBase zone = DropZones.Find(x => x.name == num);

                if (zone.SnapDropZone.GetCurrentSnappedInteractableObject() == null)
                {
                    currentObjects.Add(zone);
                    allCurrentObjects.Add(zone);
                    zone.SetColliderHighlightActive(true);
                    SetObjectArrow(zone, true);
                    activatedObjects = true;
                }
            }
            //if (currentObjects[0].CurrentSnappedObject == null)
                if (activatedObjects)
                    SetDescription(currentObjects[0].gameObject);

            if (!activatedObjects)
                Next();
        }
    }

    public void Next()
    {
        if (ListOfNames.Count > 0)
        {
            ListOfNames.Remove(ListOfNames[0]);
        }

        countBeforeAutoSnap = 0;

        if (AutoSnapValues.Count > 0)
            AutoSnapValues.Remove(AutoSnapValues[0]);

        if (ListOfNames.Count > 0)
        {
            //Вызов события для смены состояния на ProgressUI
            StateChanged();

            bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
            AutoSnappingScript.UpdateButton(flag);
            var nums = ListOfNames[0].Split(' ');
            foreach (var num in nums)
            {
                DropZoneBase zone = DropZones.Find(x => x.name == num);

                //убрать эти стрелочки у занятых объектов
                if (zone.CurrentSnappedObject == null)
                {
                    zone.SetColliderHighlightActive(true);
                    SetObjectArrow(zone, true);
                }
                
                currentObjects.Add(zone);
                allCurrentObjects.Add(zone);

                if (zone.CurrentSnappedObject != null)
                {
                    curCountOfActions++;

                    var scripts = zone.GetComponents<DraggedObject>();
                    for (int k = 0; k < scripts.Length; k++)
                    {
                        if (scripts[k].isActiveAndEnabled == false && scripts[k] != null && scripts[k].enabled == false)
                        {
                            scripts[k].swithPrefab = zone.CurrentSnappedObject;

                            scripts[k].enabled = true;
                            SetDescription(scripts[k].Key);

                            draggedObjects.Add(scripts[k]);
                            currentObjects.Remove(zone);
                            break;
                        }
                    }

                    listIsOver = true;
                }
                else
                    SetDescription(currentObjects.Last().gameObject);
            }
        }
        else
        {
            AutoSnappingScript.UpdateButton(false);
            DescriptionOn();
            SetDescription("finish");

            //Вызов события для смены состояния на ProgressUI
            StateChanged();
        }
    }

    public void Check(GameObject snappedObject)
    {
        var zone = snappedObject.GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().GetComponent<DropZoneBase>();

        if (currentObjects.Contains(zone))
        {
            snappedObject.GetComponent<ObjectsArrowController>().isClosed = true;

            countBeforeAutoSnap++;
            bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
            AutoSnappingScript.UpdateButton(flag);

            objectsToDescription.Add(snappedObject);

            if (zone.IsPortable && !PortableObjectsList.Contains(snappedObject))
                PortableObjectsList.Add(snappedObject);

            snappedObject.GetComponent<Collider>().enabled = false;
            snappedObject.GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;
            currentObjects.Remove(zone);
            zone.SetColliderHighlightActive(false);
            if (!zone.HaveAdditional)
            {
                if (currentObjects.Count == 0)
                    Next();
            }
            else
                StartCoroutine(AdditionalObjects(zone.GetComponent<NotOrderList>()));
        }
    }

    public void Check(GameObject snappedObject, DropZoneBase zone)
    {
        if (currentObjects.Contains(zone))
        {
            objectsToDescription.Add(snappedObject);

            snappedObject.GetComponent<ObjectsArrowController>().isClosed = true;

            if (zone.IsPortable && !PortableObjectsList.Contains(snappedObject))
                PortableObjectsList.Add(snappedObject);

            snappedObject.GetComponent<Collider>().enabled = false;
            snappedObject.GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;
            currentObjects.Remove(zone);
            zone.SetColliderHighlightActive(false);
            SetObjectArrow(zone, false);

            if (currentObjects.Count == 0)
                Next();
        }
    }

    IEnumerator AdditionalObjects(NotOrderList go)
    {
        AutoSnappingScript.UpdateButton(false);

        go.IsStartedScript = true;

        additional = true;

        foreach (var zone in allCurrentObjects)
        {
            zone.SetColliderHighlightActive(false);
            SetObjectArrow(zone, false);
        }

        foreach (var obj in go.PossibleObjects)
        {
            obj.GetComponent<ObjectsArrowController>().SetShowingArrow(true);
            SetDescription(obj.gameObject);
        }

        go.SetColliderHighlightActive(true);

        while (go.NextDropZones.Count > tempDroppedObjectsList.Count)
            yield return null;

        go.IsStartedScript = false;

        go.SetColliderHighlightActive(false);
        foreach (var obj in tempDroppedObjectsList)
        {
            obj.GetComponent<Collider>().enabled = false;
            obj.GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;

            if (go.NextDropZones[0].GetComponent<DropZoneBase>().IsPortable == true && !PortableObjectsList.Contains(obj))
                PortableObjectsList.Add(obj);

        }
        tempDroppedObjectsList.Clear();
        additional = false;
        foreach (var zone in allCurrentObjects)
        {
            zone.SetColliderHighlightActive(true);
            SetObjectArrow(zone, true);
            SetDescription(zone.gameObject);
        }
        foreach (var obj in go.PossibleObjects)
            obj.GetComponent<ObjectsArrowController>().SetShowingArrow(false);

        if (currentObjects.Count == 0)
            Next();

        bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
        AutoSnappingScript.UpdateButton(flag);
    }

    public override void EndActionsWithDraggedObj()
    {
        countOfActions++;
        if (countOfActions == curCountOfActions)
        {
            draggedObjects.Clear();
            countOfActions = curCountOfActions = 0;
            listIsOver = false;
            Next();
        }
    }

    public void SetObjectArrow(DropZoneBase snapDropZoneObj, bool arrowEnabled)
    {
        var objToGrab = snapDropZoneObj.Prefab;
        var grabObjectTrainingScript = objToGrab.gameObject.GetComponent<ObjectsArrowController>();

        

        if (arrowEnabled && snapDropZoneObj.SnapDropZone.GetCurrentSnappedInteractableObject() != null)
            return;

        if (grabObjectTrainingScript)
        {
            if (arrowEnabled && grabObjectTrainingScript.CantSetArrow)
                return;

            grabObjectTrainingScript.SetShowingArrow(arrowEnabled);
        }
    }

    public void AutoSnap()
    {
        var objectsToSnap = new List<DropZoneBase>(currentObjects);//(allCurrentObjects);
        //draggedObjects
        if (objectsToSnap.Count > 0)
        {
            AutoSnappingScript.AutoSnapOblects(objectsToSnap, this);
        }
        else
        {
            var draggedObjectsToSnap = new List<DropZoneBase>();
            foreach (var item in draggedObjects)
            {
                var t = item.GetComponent<DropZoneBase>();
                draggedObjectsToSnap.Add(t);
            }
            AutoSnappingScript.AutoSnapOblects(draggedObjectsToSnap, this);
        }
    }

    private void StateChanged() => OnStateChanged?.Invoke();
}