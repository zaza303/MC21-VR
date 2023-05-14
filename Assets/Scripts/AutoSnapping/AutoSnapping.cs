using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class AutoSnapping : MonoBehaviour
{
    public List<GameObject> ObjectsToSnap = new List<GameObject>();
    public List<DropZoneBase> DropZones = new List<DropZoneBase>();
    public GameObject Bolt;
    public GameObject SmallSwithBoltBolt;
    public GameObject BigSwitchBolt;
    public GameObject Boltik;
    public GameObject StringerBoltik;
    public GameObject ObodBoltik;

    [SerializeField]
    private Button SnapButton;
    [SerializeField]
    private bool SnapOnEnable;
    [SerializeField]
    private GameManager gameManager;

    private bool final = false; 
    private void Start() => UpdateButton(false);

    public void UpdateButton(bool value)
    {
        SnapButton.interactable = value;
    }


    public void AutoSnapOblects(List<DropZoneBase> dropZoneBases, DropManager dropManager)
    {
        for (int i = 0; i < dropZoneBases.Count; i++)
        {
            DropZoneBase zone = dropZoneBases[i];
            GameObject snapObject = ObjectsToSnap.Where(x => x.GetComponent<ObjectsArrowController>().isClosed == false && 
                x.tag == zone.GetComponent<VRTK_SnapDropZone>().highlightObjectPrefab.tag).FirstOrDefault();

            if (snapObject == null)
                continue;

            zone.SnapDropZone.ForceSnap(snapObject);
            zone.SetCurrentSnappedObject(snapObject);

            snapObject.GetComponent<InteractableObject>().SnapDropZone = zone.SnapDropZone;
            snapObject.GetComponent<InteractableObject>().IsSnapped = true;

            zone.SetColliderHighlightActive(false);
            if (gameManager.CurrentMode == GameMode.TrainingMode)
                ((TrainingDropManager)dropManager).SetObjectArrow(zone, false);

            if (zone.HaveAdditional)
            {
                var dropZones = zone.GetComponent<NotOrderList>().NextDropZones;

                if (dropZones.Count == 4)
                {
                    foreach (var boltZone in dropZones)
                    {
                        var bolt = Instantiate(Bolt);
                        boltZone.GetComponent<VRTK_SnapDropZone>().ForceSnap(bolt);
                        boltZone.GetComponent<DropZoneBase>().SetCurrentSnappedObject(bolt);

                        bolt.GetComponent<InteractableObject>().SnapDropZone = boltZone.GetComponent<VRTK_SnapDropZone>();
                        bolt.GetComponent<InteractableObject>().IsSnapped = true;

                        bolt.GetComponent<Collider>().enabled = false;
                        if (gameManager.CurrentMode == GameMode.CheckMode)
                        {
                            boltZone.GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                            boltZone.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                            zone.SnapDropZone.ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                            zone.SnapDropZone.ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                        }
                    }
                }
                else if (dropZones.Count == 2)
                {
                    if (dropZones[0].tag == "StringerBoltik")
                    {
                        Destroy(zone.GetComponent<NotOrderList>());
                        zone.HaveAdditional = false;

                        var boltik1 = Instantiate(StringerBoltik, snapObject.gameObject.transform);
                        dropZones[0].GetComponent<VRTK_SnapDropZone>().ForceSnap(boltik1);
                        dropZones[0].GetComponent<DropZoneBase>().SetCurrentSnappedObject(boltik1);

                        var boltik2 = Instantiate(StringerBoltik, snapObject.gameObject.transform);
                        dropZones[1].GetComponent<VRTK_SnapDropZone>().ForceSnap(boltik2);
                        dropZones[1].GetComponent<DropZoneBase>().SetCurrentSnappedObject(boltik2);

                        boltik1.GetComponent<Collider>().enabled = false;
                        boltik2.GetComponent<Collider>().enabled = false;
                    }
                    else if (dropZones[0].tag == "obodboltik")
                    {
                        Destroy(zone.GetComponent<NotOrderList>());
                        zone.HaveAdditional = false;

                        var boltik1 = Instantiate(ObodBoltik, snapObject.gameObject.transform);
                        dropZones[0].GetComponent<VRTK_SnapDropZone>().ForceSnap(boltik1);
                        dropZones[0].GetComponent<DropZoneBase>().SetCurrentSnappedObject(boltik1);

                        var boltik2 = Instantiate(ObodBoltik, snapObject.gameObject.transform);
                        dropZones[1].GetComponent<VRTK_SnapDropZone>().ForceSnap(boltik2);
                        dropZones[1].GetComponent<DropZoneBase>().SetCurrentSnappedObject(boltik2);

                        boltik1.GetComponent<Collider>().enabled = false;
                        boltik2.GetComponent<Collider>().enabled = false;


                    }
                    else
                    {
                        Destroy(zone.GetComponent<NotOrderList>());
                        zone.HaveAdditional = false;

                        var smallBolt = Instantiate(SmallSwithBoltBolt);
                        dropZones[0].GetComponent<VRTK_SnapDropZone>().ForceSnap(smallBolt);
                        dropZones[0].GetComponent<DropZoneBase>().SetCurrentSnappedObject(smallBolt);

                        var bigBolt = Instantiate(BigSwitchBolt);
                        dropZones[1].GetComponent<VRTK_SnapDropZone>().ForceSnap(bigBolt);
                        dropZones[1].GetComponent<DropZoneBase>().SetCurrentSnappedObject(bigBolt);

                        smallBolt.GetComponent<Collider>().enabled = false;
                        bigBolt.GetComponent<Collider>().enabled = false;
                    }

                    //fix DiscardWrongObj error 
                    if (gameManager.CurrentMode == GameMode.CheckMode)
                    {
                        var dropzone0Snap = dropZones[0].GetComponent<VRTK_SnapDropZone>();
                        var dropzone1Snap = dropZones[1].GetComponent<VRTK_SnapDropZone>();
                        dropzone0Snap.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                        dropzone1Snap.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                        dropzone0Snap.ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        dropzone1Snap.ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        zone.SnapDropZone.ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        zone.SnapDropZone.ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                    }
                }
                else if (dropZones.Count == 18)
                {
                    zone.GetComponent<ObodsNotOrder>().GetAllNextDropZones();
                    dropZones = zone.GetComponent<ObodsNotOrder>().NextDropZones;

                    Destroy(zone.GetComponent<ObodsNotOrder>());
                    Destroy(zone.GetComponent<ObodDrill>());
                    zone.HaveAdditional = false;

                    foreach (var boltZone in dropZones)
                    {
                        var boltik = Instantiate(Boltik);
                        boltZone.GetComponent<VRTK_SnapDropZone>().ForceSnap(boltik);
                        boltZone.GetComponent<DropZoneBase>().SetCurrentSnappedObject(boltik);
                        boltik.GetComponent<Collider>().enabled = false;

                        //fix DiscardWrongObj error 
                        if (gameManager.CurrentMode == GameMode.CheckMode)
                        {
                            boltZone.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                            boltZone.GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        }
                    }
                    if (gameManager.CurrentMode == GameMode.CheckMode)
                    {
                        zone.SnapDropZone.ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        zone.SnapDropZone.ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                    }
                }
                else
                {
                    Debug.Log("*********************");
                    Destroy(zone.GetComponent<NotOrderList>());
                    var clampScript = zone.GetComponent<SetClamp>();
                    if (clampScript)
                    {
                        //clampScript.Destroy();
                        Destroy(clampScript);
                    }
                    //GameManager.SetScore(1);
                    zone.HaveAdditional = false;

                    foreach (var boltZone in dropZones)
                    {
                        var boltik = Instantiate(Boltik, snapObject.gameObject.transform);
                        boltZone.GetComponent<VRTK_SnapDropZone>().ForceSnap(boltik);
                        boltZone.GetComponent<DropZoneBase>().SetCurrentSnappedObject(boltik);
                        boltik.GetComponent<Collider>().enabled = false;

                        //fix DiscardWrongObj error 
                        if (gameManager.CurrentMode == GameMode.CheckMode)
                        {
                            boltZone.GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                            boltZone.GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        }
                        //надо еще уалить дыры
                    }

                    if (gameManager.CurrentMode == GameMode.CheckMode)
                    {
                        zone.SnapDropZone.ObjectUnsnappedFromDropZone -= ((CheckDropManager)dropManager).ExitedSnapDropZone;
                        zone.SnapDropZone.ObjectSnappedToDropZone -= ((CheckDropManager)dropManager).EnteredSnapDropZone;
                    }
                }

                switch (gameManager.CurrentMode)
                {
                    case GameMode.TrainingMode:
                        ((TrainingDropManager)dropManager).Check(snapObject, zone);
                        ((TrainingDropManager)dropManager).SetObjectArrow(zone, false);
                        break;
                    case GameMode.CheckMode:
                        ((CheckDropManager)dropManager).SetEnteredDropZone(snapObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
