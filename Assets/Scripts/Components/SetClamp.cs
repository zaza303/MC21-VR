using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public enum State { Drill, Countersink };

public class SetClamp: MonoBehaviour
{
    [SerializeField]
    private GameObject Switch;
    [SerializeField]
    private int numperOfKnit;
    [SerializeField]
    public GameObject drill;
    [SerializeField]
    public GameObject drillCountersink;

    [SerializeField]
    private string boltikKey = "boltikknit";

    [SerializeField]
    private string drillKey = "drillknit";
    [SerializeField]
    private string drillCountersinkKey = "drillCountersinkKnit";

    private AdditionalReference clampReference;
    private List<GameObject> clamps = new List<GameObject>();
    private List<GameObject> holes = new List<GameObject>();
    private List<GameObject> holesToDestroy = new List<GameObject>();
    private DropZoneBase dropZoneBase;
    private bool activateDrill, activateCountersinkDrill;
    private ObjectsArrowController drillScript;
    private ObjectsArrowController drillCountersinkScript;
    private NotOrderList notOrderList;

    public bool Done;
    public bool Close;
    public DropZoneBase DropZoneBase;

    [SerializeField]
    public TrainingDropManager trainingDropManager;
    [SerializeField]
    public CheckDropManager checkDropManager;

    private bool canOpen;
    private int rotationSpeed = 25;
    private int sign;
    private float time;
    private bool isTraining;
    private bool wrong;
    private State currentState;

    public void Awake()
    {
        if (GetComponent<SetClamp>() != null)
        {
            GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += SetClamp_ObjectSnappedToDropZone;
            GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone += SetClamp_ObjectUnsnappedFromDropZone;
            notOrderList = GetComponent<NotOrderList>();
            DropZoneBase = GetComponent<DropZoneBase>();
            drillScript = drill.GetComponent<ObjectsArrowController>();
            drillCountersinkScript = drillCountersink.GetComponent<ObjectsArrowController>();
            time = 0;
            Done = Close = activateDrill = activateCountersinkDrill = wrong = false;
            sign = 1;
            rotationSpeed = 70;
            dropZoneBase = GetComponent<DropZoneBase>();
            currentState = State.Drill;
        }
    }

    private void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        if (dropZoneBase.CurrentSnappedObject)
        {
            var list = dropZoneBase.CurrentSnappedObject.GetComponent<AdditionalReference>().Clamps;
            foreach (var item in list)
            {
                if (item)
                    item.GetComponent<Hole>().DestroySelf();
            }
        }

        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= SetClamp_ObjectSnappedToDropZone;
        GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone -= SetClamp_ObjectUnsnappedFromDropZone;
    }

    private void SetClamp_ObjectUnsnappedFromDropZone(object sender, SnapDropZoneEventArgs e)
    {
        canOpen = false;
        wrong = true;
    }

    private void SetClamp_ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
    {
       if (GetComponent<SetClamp>() != null)
       {
            GetComponent<SetClamp>().enabled = true;
            time = 0;
            StartCoroutine(StartAgain());
       }
    }

    IEnumerator StartAgain()
    {
        yield return new WaitForSeconds(.2f);
        if (!canOpen && !wrong && holes.Count == 0)
            Start();
    }

    public void Start()
    {
        if (Switch.GetComponent<DropZoneBase>().CurrentSnappedObject != null)
        {
            clampReference = Switch.GetComponent<DropZoneBase>().CurrentSnappedObject.GetComponent<AdditionalReference>();
            holesToDestroy = dropZoneBase.CurrentSnappedObject.GetComponent<AdditionalReference>().Clamps;
            holes = new List<GameObject>(holesToDestroy);

            isTraining = trainingDropManager.gameObject.activeSelf;
            time = 0;
            RotateClamps();

            foreach (var obj in notOrderList.PossibleObjects)
            {
                obj.GetComponent<ObjectsArrowController>().SetShowingArrow(false);
            }
        }
    }

    private void RotateClamps()
    {
        var num = numperOfKnit * 2;
        clamps.Add(clampReference.Clamps[num]);
        clamps.Add(clampReference.Clamps[num + 1]);

        canOpen = true;
    }

    void Update()
    {
        if (!Close)
            notOrderList.SetColliderHighlightActive(false);

        if (canOpen)
        {
            if (time < 180)
            {
                if (isTraining)
                    trainingDropManager.SetDescription(drillKey);
                foreach (var item in clamps)
                    item.transform.Rotate(new Vector3(0, sign * rotationSpeed * Time.deltaTime, 0));
                time += rotationSpeed * Time.deltaTime;
            }
            else
            {
                if (!Done)
                    GetHoles();
            }

            if ((holes.TrueForAll(x => x != null && x.GetComponent<Renderer>().material.color == Color.yellow 
                    && x.GetComponent<Hole>().WasDrill == false) ||
                holes.TrueForAll(x => x == null)) && !Close)
                End();
        }
        else
        {
            if (wrong)
            {
                if (time > 0)
                {
                    foreach (var item in clamps)
                        item.transform.Rotate(new Vector3(0, -sign * rotationSpeed * Time.deltaTime, 0));
                    time -= rotationSpeed * Time.deltaTime;
                }
                else
                {
                    wrong = false;
                    GetComponent<SetClamp>().enabled = false;
                }
            }
        }

        if (activateDrill && !drillScript.grabbed && !activateCountersinkDrill)
        {
            if (isTraining)
            {
                drillScript.SetShowingArrow(true);
                drillCountersinkScript.SetShowingArrow(false);

                trainingDropManager.SetDescription(drillKey);
            }
        }

        if (activateCountersinkDrill && !drillCountersinkScript.grabbed && !activateDrill)
        {
            if (isTraining)
            {
                drillCountersinkScript.SetShowingArrow(true);
                drillScript.SetShowingArrow(false);

                trainingDropManager.SetDescription(drillCountersinkKey);
            }
        }

        if (activateDrill && isTraining)
            trainingDropManager.SetDescription(drillKey);

        if (activateCountersinkDrill && isTraining)
            trainingDropManager.SetDescription(drillCountersinkKey);
    }

    private void GetHoles()
    {
        Done = true;
        activateDrill = true;
        foreach (var item in holes)
        {
            item.GetComponent<Renderer>().material.color = Color.green;
            item.GetComponent<Collider>().enabled = true;
        }
    }

    void End()
    {
        switch (currentState)
        {
            case State.Drill:

                foreach (var item in holes)
                {
                    var hole = item.GetComponent<Hole>();
                    hole.SetMaxTime();
                    hole.WasDrill = true;
                }

                activateCountersinkDrill = true;
                activateDrill = false;

                currentState = State.Countersink;
                drill.GetComponent<Drill>().CurrentState = currentState;
                drillCountersink.GetComponent<Drill>().CurrentState = currentState;

                break;
            case State.Countersink:

                Close = true;
                activateCountersinkDrill = false;

                if (isTraining)
                {
                    notOrderList.SetColliderHighlightActive(true);
                    trainingDropManager.SetDescription(boltikKey);
                    foreach (var obj in notOrderList.PossibleObjects)
                    {
                        obj.GetComponent<ObjectsArrowController>().SetShowingArrow(true);
                    }
                    drillCountersinkScript.SetShowingArrow(false);
                    drillScript.SetShowingArrow(false);
                }
                else
                {
                    notOrderList.SetColliderActive(true);
                    notOrderList.SetColliderHighlightActive(true);

                }

                activateDrill = false;
                drillScript.SetShowingArrow(false);
                drill.GetComponent<Drill>().CurrentState = State.Drill;
                break;
            default:
                break;
        }
    }
}
