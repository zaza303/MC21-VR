using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class ObodDrill : DraggedObject
{
    [SerializeField]
    public GameObject drill;
    [SerializeField]
    public GameObject drillCountersink;
    public List<GameObject> Holes = new List<GameObject>();
    private bool activateDrill, activateCountersinkDrill;
    private ObjectsArrowController drillScript;
    private ObjectsArrowController drillCountersinkScript;

    [SerializeField]
    private string boltikKey = "boltikscob";

    [SerializeField]
    private string drillKey = "drillknit";
    [SerializeField]
    private string drillCountersinkKey = "drillCountersinkKnit";

    [SerializeField]
    private NotOrderList notOrderList;

    private bool openn;
    private bool isTraining;
    private bool wrong;
    private State currentState;

    public new void Awake()
    {
        DropZoneBase = GetComponent<DropZoneBase>();
        drillScript = drill.GetComponent<ObjectsArrowController>();
        drillCountersinkScript = drillCountersink.GetComponent<ObjectsArrowController>();
        Done = Close = activateDrill = activateCountersinkDrill = wrong = false;
        currentState = State.Drill;
    }

    private void OnDestroy()
    {
        Destroy();
        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= SetClamp_ObjectSnappedToDropZone;
        GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone -= SetClamp_ObjectUnsnappedFromDropZone;
    }

    public void Destroy()
    {
        foreach (var hole in Holes)
        {
            if (hole)
                hole.GetComponent<Hole>().DestroySelf();
        }
    }

    private void SetClamp_ObjectUnsnappedFromDropZone(object sender, SnapDropZoneEventArgs e)
    {
        openn = false;
        wrong = true;
    }

    private void SetClamp_ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
    {
        if (GetComponent<SetClamp>() != null)
        {
            GetComponent<SetClamp>().enabled = true;
            StartCoroutine(StartAgain());
        }
    }

    IEnumerator StartAgain()
    {
        yield return new WaitForSeconds(.2f);
        if (!openn && !wrong && Holes.Count == 0)
            Start();
    }

    public new void Start()
    {
        drill.GetComponent<Drill>().Awake();
        drillCountersink.GetComponent<Drill>().Awake();
        isTraining = trainingDropManager.gameObject.activeSelf;

        foreach (var item in Holes)
        {
            item.GetComponent<Renderer>().enabled = true;
        }
        notOrderList.enabled = true;
        RotateClamps();

        foreach (var obj in notOrderList.PossibleObjects)
        {
            obj.GetComponent<ObjectsArrowController>().SetShowingArrow(false);
        }
    }

    private void RotateClamps() => openn = true;

    void Update()
    {
        if (!Close)
            notOrderList.SetColliderHighlightActive(false);

        if (openn)
        {
            if (!Done)
                GetHoles();

            if ((Holes.TrueForAll(x => x != null && x.GetComponent<Renderer>().material.color == Color.yellow
                    && x.GetComponent<Hole>().WasDrill == false) ||
                Holes.TrueForAll(x => x == null)) && !Close)
                End();
        }
        else
        {
            if (wrong)
            {
                wrong = false;
                GetComponent<SetClamp>().enabled = false;
            }
        }

        if (activateDrill && !drillScript.grabbed)
        {
            if (isTraining)
            {
                drillScript.SetShowingArrow(true);
                trainingDropManager.SetDescription(drillKey);
            }
        }

        if (activateCountersinkDrill && !drillCountersinkScript.grabbed)
        {
            if (isTraining)
            {
                drillCountersinkScript.SetShowingArrow(true);
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
                    notOrderList.SetColliderActive(true);
                GameManager.SetScore(1);
                activateDrill = false;
                //drillScript.SetShowingArrow(false);
                break;
            default:
                break;
        }
    }

    public void EndAllScript()
    {
        for (int i = 0; i < Holes.Count; i++)
        {
            Destroy(Holes[i]);
        }
    }
}
