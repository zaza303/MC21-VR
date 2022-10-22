using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System.Linq;

public class CheckDropManager : DropManager
{
    public List<GameObject> DroppedObjectsList = new List<GameObject>();
    private List<List<GameObject>> ObjectsToDrop = new List<List<GameObject>>();
    private List<List<DropZoneBase>> ZonesToDrop = new List<List<DropZoneBase>>();
    public List<GameObject> tempDroppedObjectsList = new List<GameObject>();
    private List<DropZoneBase> tempZonesToDrop = new List<DropZoneBase>();
    private List<List<GameObject>> actionsWithDroppedObj = new List<List<GameObject>>();
    private List<string> tempListOfNames = new List<string>();

    public List<DropZoneBase> SnappedDropZones = new List<DropZoneBase>();

    private int i = 0;

    private int countBeforeAutoSnap = 0;

    protected override void Awake()
    {
        base.Awake();
        tempListOfNames = new List<string>(ListOfNames);
        additional = false;
        listIsOver = false;

        AutoSnappingScript.UpdateButton(false);
    }

    private void Start()
    {
        foreach (var item in DropZones)
        {
            item.SnapDropZone.ObjectSnappedToDropZone += EnteredSnapDropZone;
            item.SnapDropZone.ObjectUnsnappedFromDropZone += ExitedSnapDropZone;
        }

        foreach (var item in SnappedDropZones)
        {
            item.SnapDropZone.ObjectSnappedToDropZone -= EnteredSnapDropZone;
            item.SnapDropZone.ObjectUnsnappedFromDropZone -= ExitedSnapDropZone;

            item.GetComponent<Collider>().enabled = false;
        }

        FindObjectsInNamesList();
    }

    bool isStart = false;


    public void FindObjectsInNamesList()
    {
        bool stop = false;
        int lastIndex = 0;

        countBeforeAutoSnap = 0;
        AutoSnapValues.Remove(AutoSnapValues[0]);
        if (AutoSnapValues.Count > 0)
        {
            bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
            AutoSnappingScript.UpdateButton(flag);
        }

        if (tempListOfNames.Count == 0)
        {
            GameOver();
        }

        if (Scenario == Constants.ScenarioTwo && !isStart)
        {
            string name = ListOfNames[0];
            var nums = name.Split(' ');

            List<GameObject> tempList = new List<GameObject>();
            foreach (var num in nums)
            {
                var curObject = DropZones.Find(x => x.name == num);
                var prefab = curObject.SnapDropZone.defaultSnappedInteractableObject.gameObject;
                DroppedObjectsList.Add(prefab);
                if (!objectsToDescription.Contains(prefab))
                    objectsToDescription.Add(prefab);

                tempList.Add(prefab);
            }
            ObjectsToDrop.Add(tempList);

            tempListOfNames.RemoveAt(0);
            isStart = true;

            i++;
        }
        else if (Scenario == Constants.ScenarioOne && !isStart)
        {
            List<GameObject> tempList = new List<GameObject>();
            var prefab = SnappedDropZones[0].GetComponent<VRTK_SnapDropZone>().defaultSnappedInteractableObject.gameObject;
            
            if (!objectsToDescription.Contains(prefab))
                objectsToDescription.Add(prefab);

            tempList.Add(prefab);
            ObjectsToDrop.Add(tempList);
            //для исправления ошибки в Check
            ZonesToDrop.Add(new List<DropZoneBase>());
            isStart = true;

            i++;
        }

        for (int k = 0; k < tempListOfNames.Count; k++)
        {
            string name = tempListOfNames[k];
            var nums = name.Split(' ');

            if (nums.Length == 1)
            {
                var currentObject = DropZones.Find(x => x.name == name);
                if (currentObject)
                {
                    var oneobj = new List<GameObject> { currentObject.Prefab };
                    if (!ContainsList(ObjectsToDrop, oneobj))
                        ObjectsToDrop.Add(oneobj);
                    else
                    {
                        actionsWithDroppedObj.Add(new List<GameObject> { currentObject.gameObject });
                        stop = true;
                        lastIndex = k;
                    }

                    var t = new List<DropZoneBase> { currentObject };
                    if (!ContainsDropZoneList(ZonesToDrop, t))
                        ZonesToDrop.Add(t);
                }
            }
            else
            {
                List<GameObject> tempList = new List<GameObject>();
                List<DropZoneBase> tempZonesList = new List<DropZoneBase>();
                List<GameObject> actionsWithDroppedList = new List<GameObject>();

                foreach (var num in nums)
                {
                    var curObject = DropZones.Find(x => x.name == num);

                    if (curObject)
                    {
                        tempList.Add(curObject.Prefab);
                        tempZonesList.Add(curObject);
                        actionsWithDroppedList.Add(curObject.gameObject);

                        Debug.Log(curObject.gameObject.name);
                    }
                }

                if (!ContainsList(ObjectsToDrop, tempList))
                    ObjectsToDrop.Add(tempList);
                else
                {
                    actionsWithDroppedObj.Add(actionsWithDroppedList);
                    stop = true;
                    lastIndex = k;
                }

                if (!ContainsDropZoneList(ZonesToDrop, tempZonesList))
                    ZonesToDrop.Add(tempZonesList);
            }

            if (stop)
                break;
        }

        for (int l = 0; l < lastIndex + 1; l++)
        {
            if (tempListOfNames.Count > 0)
                tempListOfNames.RemoveAt(0);
        }

        listIsOver = false;
        CheckActionsWithDragged();
    }

    private void GameOver()
    {
        AutoSnappingScript.UpdateButton(false);
        DescriptionOn();
        GameObject.FindObjectOfType<GameManager>().SetResult();
        SetSoundCheckPanel(Constants.CheckMode);
    }

    public override void EnteredSnapDropZone(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.GetComponent<InteractableObject>().IsSnapped)
            return;
        if (listIsOver == false)
        {
            if (!additional)
            {
                DroppedObjectsList.Add(e.snappedObject);
                if (!objectsToDescription.Contains(e.snappedObject))
                    objectsToDescription.Add(e.snappedObject);

                Check();
            }
            else
                tempDroppedObjectsList.Add(e.snappedObject);
        }
        else
            /////////???????????
            DiscardWrongObj(e.snappedObject);
    }

    public void SetEnteredDropZone(GameObject snappedObject)
    {
        DroppedObjectsList.Add(snappedObject);
        if (!objectsToDescription.Contains(snappedObject))
            objectsToDescription.Add(snappedObject);

        Check();
    }

    public override void ExitedSnapDropZone(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.GetComponent<InteractableObject>().IsSnapped)
            return;
        if (listIsOver == false)
        {
            if (DroppedObjectsList.Contains(e.snappedObject))
            {
                DroppedObjectsList.Remove(e.snappedObject);
                objectsToDescription.Add(e.snappedObject);
            }
            else
                tempDroppedObjectsList.Remove(e.snappedObject);
        }
    }

    private void Check()
    {
        int count = 0;
        if (ObjectsToDrop[i].Count == 1)
        {
            count = DroppedObjectsList.Count - 1;
            if (ObjectsToDrop[i][0] == DroppedObjectsList[count])
            {
                if (ZonesToDrop[i][0].GetComponent<VRTK_SnapDropZone>().highlightObjectPrefab != DroppedObjectsList[count] ||
                    DroppedObjectsList[count].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone()
                    != ZonesToDrop[i][0].GetComponent<VRTK_SnapDropZone>())
                {
                    DiscardWrongObj(DroppedObjectsList[count]);
                    DroppedObjectsList.RemoveAt(count);
                }
                else
                {
                    var snapzone = DroppedObjectsList[count].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone();
                    if (DroppedObjectsList[count].GetComponent<Collider>().enabled == true)
                        GameManager.SetScore(1);

                    DroppedObjectsList[count].GetComponent<Collider>().enabled = false;
                    DroppedObjectsList[count].GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;

                    if (snapzone.GetComponent<DropZoneBase>().IsPortable == true && !PortableObjectsList.Contains(DroppedObjectsList[count]))
                        PortableObjectsList.Add(DroppedObjectsList[count]);

                    if (DroppedObjectsList[count].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone())
                    {
                        if (DroppedObjectsList[count].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().GetComponent<NotOrderList>())
                        {
                            additional = true;

                            StartCoroutine(AdditionalObjects(DroppedObjectsList[count].
                                GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().GetComponent<NotOrderList>()));
                        }
                    }

                    DroppedObjectsList.Clear();
                    if (tempListOfNames.Count == 0)
                        GameOver();

                    i++;
                    count++;
                    countBeforeAutoSnap = 0;
                    AutoSnapValues.Remove(AutoSnapValues[0]);
                    if (AutoSnapValues.Count > 0)
                    {
                        bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
                        AutoSnappingScript.UpdateButton(flag);
                    }

                }
            }
            else
            {
                DiscardWrongObj(DroppedObjectsList[count]);
                count--;
                DroppedObjectsList.RemoveAt(count);
            }
        }
        else
        {
            for (int j = 0; j < DroppedObjectsList.Count; j++)
            {
                if (!ContainsTag(ObjectsToDrop[i], DroppedObjectsList[j].tag))
                {
                    DiscardWrongObj(DroppedObjectsList[j], false);
                    DroppedObjectsList.RemoveAt(j);
                }
                else
                {
                    countBeforeAutoSnap++;
                    if (AutoSnapValues.Count > 0)
                    {
                        bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
                        AutoSnappingScript.UpdateButton(flag);
                    }

                    ///ошибка при автоснеппинге
                    var snapzone = DroppedObjectsList[j].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone();
                    if (snapzone == null)
                    {
                        snapzone = DroppedObjectsList[j].GetComponent<InteractableObject>().SnapDropZone;

                        if (DroppedObjectsList[j].GetComponent<Collider>().enabled == true)
                            GameManager.SetScore(1);

                        if (snapzone.GetComponent<DropZoneBase>().IsPortable == true && !PortableObjectsList.Contains(DroppedObjectsList[j]))
                            PortableObjectsList.Add(DroppedObjectsList[j]);
                        
                        //для поиска объекта в автоснеппинге
                        DroppedObjectsList[j].GetComponent<ObjectsArrowController>().isClosed = true;
                        
                        DroppedObjectsList[j].GetComponent<Collider>().enabled = false;
                        DroppedObjectsList[j].GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;

                        if (snapzone.gameObject.GetComponent<SetClamp>() != null)
                            snapzone.gameObject.GetComponent<SetClamp>().enabled = true;
                    }
                    else
                    {
                        if (DroppedObjectsList[j].GetComponent<Collider>().enabled == true)
                            GameManager.SetScore(1);

                        if (snapzone.GetComponent<DropZoneBase>().IsPortable == true && !PortableObjectsList.Contains(DroppedObjectsList[j]))
                            PortableObjectsList.Add(DroppedObjectsList[j]);

                        //для поиска объекта в автоснеппинге
                        DroppedObjectsList[j].GetComponent<ObjectsArrowController>().isClosed = true;

                        DroppedObjectsList[j].GetComponent<Collider>().enabled = false;
                        DroppedObjectsList[j].GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;

                        if (DroppedObjectsList[j].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().gameObject.GetComponent<SetClamp>() != null)
                            DroppedObjectsList[j].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().gameObject.GetComponent<SetClamp>().enabled = true;

                        if (DroppedObjectsList[j].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone())
                        {
                            if (DroppedObjectsList[j].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().GetComponent<NotOrderList>())
                            {
                                additional = true;

                                StartCoroutine(AdditionalObjects(DroppedObjectsList[j].
                                    GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().GetComponent<NotOrderList>()));
                            }
                        }
                    }
                    
                }

                if (ObjectsToDrop[i].Count == DroppedObjectsList.Count && j == DroppedObjectsList.Count - 1)
                {
                    i++;
                    DroppedObjectsList.Clear();

                    countBeforeAutoSnap = 0;
                    AutoSnapValues.Remove(AutoSnapValues[0]);
                    if (AutoSnapValues.Count > 0)
                    {
                        bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
                        AutoSnappingScript.UpdateButton(flag);
                    }

                    //////////////////////////////if (tempListOfNames.Count == 0)
                    if (tempListOfNames.Count == 0 && actionsWithDroppedObj.Count == 0)
                        GameOver();
                    else if (tempListOfNames.Count == 0)
                        CheckActionsWithDragged();

                    break;
                }
            }
        }

        CheckActionsWithDragged();
    }

    private void CheckActionsWithDragged()
    {
        if (ObjectsToDrop.TrueForAll(x => x.TrueForAll(y => !y.GetComponent<Collider>().enabled)) && !additional)
        {
            listIsOver = true;

            foreach (var item in actionsWithDroppedObj)
            {
                foreach (var t in item)
                {
                    var scripts = t.GetComponents<DraggedObject>();
                    for (int k = 0; k < scripts.Length; k++)
                    {
                        if (scripts[k].enabled == false)
                        {
                            var scrNorOrder = t.GetComponent<NotOrderList>();

                            if (scrNorOrder != null && t.tag != "Obod")
                            {
                                if (scrNorOrder.notReady)
                                {
                                    break;
                                }
                            }
                            scripts[k].enabled = true;
                            draggedObjects.Add(scripts[k]);
                            break;
                        }
                    }
                }
            }
        }
    }

    public override void EndActionsWithDraggedObj()
    {
        countOfActions++;
        if (countOfActions == actionsWithDroppedObj[0].Count)
        {
            GameManager.SetScore(1);
            draggedObjects.Clear();
            actionsWithDroppedObj.Clear();
            countOfActions = 0;
            listIsOver = false;
            FindObjectsInNamesList();
        }
    }

    private bool ContainsTag(List<GameObject> tagsList, string tag)
    {
        foreach (var item in tagsList)
        {
            if (item.tag == tag)
                return true;
        }

        return false;
    }

    IEnumerator AdditionalObjects(NotOrderList go)
    {
        additional = true;
        AutoSnappingScript.UpdateButton(false);

        if (go is ObodsNotOrder)
        {
            ((ObodsNotOrder)go).GetAllNextDropZones();
        }

        tempZonesToDrop.AddRange(from item in go.NextDropZones
                                 where item != null
                                 select item.GetComponent<DropZoneBase>());
        if (go is ObodsNotOrder)
        {
            ((ObodsNotOrder)go).RemoveNextDropZones();
        }


        while (go.NextDropZones.Count > 0)
        {
            if (tempDroppedObjectsList.Count > 0)
            {
                if (ContainsTag(go.PossibleObjects, tempDroppedObjectsList[0].tag)
                    && go.NextDropZones.Contains(tempDroppedObjectsList[0].
                    GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().gameObject))
                {
                    tempDroppedObjectsList[0].GetComponent<Collider>().enabled = false;
                    tempDroppedObjectsList[0].GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;
                    //GameManager.SetScore(1);

                    if (go.NextDropZones[0].GetComponent<DropZoneBase>().IsPortable == true && !PortableObjectsList.Contains(tempDroppedObjectsList[0]))
                        PortableObjectsList.Add(tempDroppedObjectsList[0]);

                    go.NextDropZones.Remove(tempDroppedObjectsList[0].GetComponent<VRTK_InteractableObject>().GetStoredSnapDropZone().gameObject);
                    tempDroppedObjectsList.RemoveAt(0);
                }
                else
                {
                    DiscardWrongObj(tempDroppedObjectsList[0], false);
                    tempDroppedObjectsList.RemoveAt(0);
                }
            }    

            yield return null;
        }
        tempDroppedObjectsList.Clear();
        additional = false;
        if (AutoSnapValues.Count > 0)
        {
            bool flag = countBeforeAutoSnap >= maxCountBeforeAutoSnap && AutoSnapValues[0];
            AutoSnappingScript.UpdateButton(flag);
        }
        GameManager.SetScore(1);
        CheckActionsWithDragged();
    }

    private void DiscardWrongObj(GameObject gameObject, bool discardAll = true)
    {
        StartCoroutine(Discard(gameObject));
        GameManager.SetScore(-1);

        IEnumerator Discard(GameObject go)
        {
            bool flag = false;
            int i = 0;

            if (discardAll)
            {
                for (int j = 0; j < ObjectsToDrop.Count; j++)
                {
                    List<GameObject> list = ObjectsToDrop[j];
                    if (list.Count > 1)
                        if (list.Contains(go))
                        {
                            i = j;
                            flag = true;
                        }
                }   
            }

            if (flag)
            {
                foreach (var obj in ObjectsToDrop[i])
                    obj.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
                go.GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(1);
        }
    }

    public void AutoSnap()
    {
        var dropZonesToAutoSnap = new List<DropZoneBase>();
        if (i >= ZonesToDrop.Count)
        {
            i = ZonesToDrop.Count - 1;
            dropZonesToAutoSnap = new List<DropZoneBase>(ZonesToDrop[i]);

            foreach (var item in dropZonesToAutoSnap)
            {
                //GameManager.SetScore(1);

                if (item.GetComponent<ObodDrill>() != null)
                {
                    GameManager.SetScore(1);
                }
            }
        }
        else
        {
            dropZonesToAutoSnap = new List<DropZoneBase>(ZonesToDrop[i]);
            for (int n = 0; n < dropZonesToAutoSnap.Count; n++)
            {
                if (dropZonesToAutoSnap[n].CurrentSnappedObject != null)
                {
                    dropZonesToAutoSnap.RemoveAt(n);
                }
            }

            foreach (var item in dropZonesToAutoSnap)
            {
                //GameManager.SetScore(1);

                if (item.GetComponent<NotOrderList>() != null)
                {
                    GameManager.SetScore(1);
                }
                if (item.GetComponent<SetClamp>() != null)
                {
                    GameManager.SetScore(1);
                }
            }
        }
        AutoSnappingScript.AutoSnapOblects(dropZonesToAutoSnap, this);
    }
}
