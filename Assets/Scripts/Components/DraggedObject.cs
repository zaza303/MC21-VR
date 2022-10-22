using UnityEngine;
using VRTK;
using VRTK.UnityEventHelper;

public abstract class DraggedObject : MonoBehaviour
{
    public bool Done;
    public bool Close;
    public DropZoneBase DropZoneBase;
    public VRTK_SnapDropZone snapDropZoneObjToUngrab;
    public GameObject ObjectToUngrab;

    public string Key;

    [SerializeField]
    public TrainingDropManager trainingDropManager;
    [SerializeField]
    public CheckDropManager checkDropManager;

    protected bool canOpen;
    protected GameObject prefab;
    public GameObject swithPrefab;
    protected int rotationSpeed = 45;
    protected int sign;
    protected float time;

    public virtual void Awake()
    {
        swithPrefab = GetComponent<VRTK_SnapDropZone>().GetCurrentSnappedObject();
        if (!swithPrefab)
        {
            if (GetComponent<VRTK_SnapDropZone>().defaultSnappedInteractableObject != null)
            {
                swithPrefab = GetComponent<VRTK_SnapDropZone>().defaultSnappedInteractableObject.gameObject;
            }
        }

        var t = ObjectToUngrab.GetComponent<VRTK_SnapDropZone>().GetCurrentSnappedObject();
        prefab = (t != null) ? t : ObjectToUngrab.GetComponent<DropZoneBase>().CurrentSnappedObject;//ObjectToUngrab.GetComponent<VRTK_SnapDropZone>().GetCurrentSnappedObject(); 

        if (!prefab)
        {
            if (ObjectToUngrab.GetComponent<VRTK_SnapDropZone>().defaultSnappedInteractableObject != null)
                prefab = ObjectToUngrab.GetComponent<VRTK_SnapDropZone>().defaultSnappedInteractableObject.gameObject;
        }

        DropZoneBase = GetComponent<DropZoneBase>();
        snapDropZoneObjToUngrab = ObjectToUngrab.GetComponent<VRTK_SnapDropZone>();
        sign = 1;
        time = 0;
        Done = Close = false;
    }

    public virtual void Start()
    {
        ObjectToUngrab.GetComponent<Collider>().enabled = true;

        canOpen = false;

        snapDropZoneObjToUngrab.ObjectSnappedToDropZone += Grabbed;
        snapDropZoneObjToUngrab.ObjectUnsnappedFromDropZone += UnGrabbed;

        if (!prefab)
        {
            var t = ObjectToUngrab.GetComponent<VRTK_SnapDropZone>().GetCurrentSnappedObject();
            prefab = (t != null) ? t : ObjectToUngrab.GetComponent<DropZoneBase>().CurrentSnappedObject;
        }

        if (!swithPrefab)
        {
            swithPrefab = GetComponent<VRTK_SnapDropZone>().GetCurrentSnappedObject();
        }

        prefab.GetComponent<Collider>().enabled = true;
        prefab.GetComponent<VRTK_InteractObjectHighlighter>().enabled = true;
    }

    public virtual void Grabbed(object sender, SnapDropZoneEventArgs e)
    {
        canOpen = false;
        if (Done)
        {
            //fix
            prefab = ObjectToUngrab.GetComponent<VRTK_SnapDropZone>().GetCurrentSnappedObject();

            swithPrefab.GetComponent<Collider>().enabled = false;
            prefab.GetComponent<Collider>().enabled = false;
            prefab.GetComponent<VRTK_InteractObjectHighlighter>().enabled = false;

            ObjectToUngrab.GetComponent<Collider>().enabled = false;
            snapDropZoneObjToUngrab.ObjectSnappedToDropZone -= Grabbed;
            snapDropZoneObjToUngrab.ObjectUnsnappedFromDropZone -= UnGrabbed;

            Close = true;

            if (trainingDropManager.gameObject.activeSelf)
                trainingDropManager.EndActionsWithDraggedObj();
            else
                checkDropManager.EndActionsWithDraggedObj();
        }
    }

    public virtual void UnGrabbed(object sender, SnapDropZoneEventArgs e) => canOpen = true;
}
