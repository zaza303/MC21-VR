using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class ObodsNotOrder : NotOrderList
{
    int countOfSnap;

    public List<GameObject> TempNextZonesList = new List<GameObject>();
    private int countOfFirstOrder = 2;


    [SerializeField]
    private string boltikKey = "boltikknit";
    [SerializeField]
    private string drillKey = "drillknit";
    [SerializeField]
    private string drillCountersinkKey = "drillCountersinkKnit";

    //из ObodDrill
    [SerializeField]
    public GameObject drill;
    [SerializeField]
    public GameObject drillCountersink;
    public List<GameObject> Holes = new List<GameObject>();
    private bool activateDrill, activateCountersinkDrill;
    private ObjectsArrowController drillScript;
    private ObjectsArrowController drillCountersinkScript;
    private bool openn;
    private bool wrong;
    private State currentState;

    public bool Done;
    public bool Close;
    public string Key;
    //

    private bool isSecondPossibleObject = false;
    private void Awake()
    {
        foreach (var item in NextDropZones)
        {
            item.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += TestNotOrder_ObjectSnappedToDropZone;
        }

        drillScript = drill.GetComponent<ObjectsArrowController>();
        drillCountersinkScript = drillCountersink.GetComponent<ObjectsArrowController>();
        Done = Close = activateDrill = activateCountersinkDrill = wrong = openn = false;
        currentState = State.Drill;
    }

    private void OnDestroy()
    {
        foreach (var item in NextDropZones)
        {
            if (item)
                item.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= TestNotOrder_ObjectSnappedToDropZone;
        }
    }

    public override void Start()
    {
        base.Start();

        TempNextZonesList = new List<GameObject>(NextDropZones);

        for (int i = countOfFirstOrder; i < NextDropZones.Count; i++)
        {
            SetColliderHighlightActive(NextDropZones[i], false);
            NextDropZones[i] = null;
        }

        if (PossibleObjects.Count > 1)
        {
            PossibleObjects[0]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
            PossibleObjects[1]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
        }
    }

    private void TestNotOrder_ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
    {
        countOfSnap++;
        Debug.Log("*************************** countOfSnap " + countOfSnap);
        if (countOfSnap == countOfFirstOrder)
        {
            StartDrill();
        }
    }

    public void SetColliderHighlightActive(GameObject gameObject, bool active)
    {
        gameObject.GetComponent<Collider>().enabled = active;
        gameObject.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = active;
    }

    ///из ObodDrill
    private void StartDrill()
    {
        Debug.Log("*************************** StartDrill ");

        drill.GetComponent<Drill>().Awake();
        drillCountersink.GetComponent<Drill>().Awake();
        isTraining = trainingropManager.gameObject.activeSelf;

        foreach (var item in Holes)
        {
            item.GetComponent<Renderer>().enabled = true;
        }

        openn = true;
    }

    public void GetAllNextDropZones()
    {
        //NextDropZones = new List<GameObject>(TempNextZonesList);
        for (int i = countOfFirstOrder; i < NextDropZones.Count; i++)
        {
            NextDropZones[i] = TempNextZonesList[i];
        }
    }

    public void RemoveNextDropZones()
    {
        for (int i = countOfFirstOrder; i < NextDropZones.Count; i++)
        {
            SetColliderHighlightActive(NextDropZones[i], false);
            NextDropZones[i] = null;
        }
    }

    void Update()
    {
        if (openn)
        {
            if (!Done)
                GetHoles();

            if ((Holes.TrueForAll(x => x != null && x.GetComponent<Renderer>().material.color == Color.yellow
                    && x.GetComponent<Hole>().WasDrill == false) ||
                Holes.TrueForAll(x => x == null)) && !Close)
                End();

            

            if (activateDrill && !drillScript.grabbed && !activateCountersinkDrill)
            {
                if (isTraining)
                {
                    drillScript.SetShowingArrow(true);
                    drillCountersinkScript.SetShowingArrow(false);

                    trainingropManager.SetDescription(drillKey);

                    if (PossibleObjects.Count > 1)
                    {
                        PossibleObjects[0]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
                        PossibleObjects[1]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
                    }
                }
            }

            if (activateCountersinkDrill && !drillCountersinkScript.grabbed && !activateDrill)
            {
                if (isTraining)
                {
                    drillCountersinkScript.SetShowingArrow(true);
                    drillScript.SetShowingArrow(false);

                    trainingropManager.SetDescription(drillCountersinkKey);

                    if (PossibleObjects.Count > 1)
                    {
                        PossibleObjects[0]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
                        PossibleObjects[1]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
                    }
                }
            }

            if (activateDrill && isTraining)
                trainingropManager.SetDescription(drillKey);

            if (activateCountersinkDrill && isTraining)
                trainingropManager.SetDescription(drillCountersinkKey);
        }

        if (!activateDrill && !activateCountersinkDrill && IsStartedScript && !isSecondPossibleObject)
        {
            PossibleObjects[1]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
        }
    }

    private void GetHoles()
    {
        Done = true;
        activateDrill = true;
        foreach (var item in Holes)
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
                foreach (var item in Holes)
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
                isSecondPossibleObject = true;
                if (isTraining)
                {
                    for (int i = countOfFirstOrder; i < NextDropZones.Count; i++)
                    {
                        NextDropZones[i] = TempNextZonesList[i];
                        SetColliderHighlightActive(NextDropZones[i], true);
                    }
                    trainingropManager.SetDescription(boltikKey);

                    if (PossibleObjects.Count > 1)
                    {
                        PossibleObjects[0]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(false);
                        PossibleObjects[1]?.GetComponent<ObjectsArrowController>()?.SetShowingArrow(true);
                    }

                    drillScript.SetShowingArrow(false);
                    drillCountersinkScript.SetShowingArrow(false);
                }
                else
                {
                    for (int i = 0; i < NextDropZones.Count; i++)
                    {
                        NextDropZones[i] = TempNextZonesList[i + 2];
                        SetColliderHighlightActive(NextDropZones[i], true);
                    }
                }

                GameManager.SetScore(1);
                activateDrill = false;
                //drillScript.SetShowingArrow(false);
                break;
            default:
                break;
        }
    }
}
