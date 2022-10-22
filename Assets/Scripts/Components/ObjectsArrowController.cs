using UnityEngine;
using VRTK;

public class ObjectsArrowController : MonoBehaviour
{
    [SerializeField]
    private GameObject Arrow;
    private GameObject arr;
    private VRTK_InteractableObject interactableObjScript;

    private bool arrowEnabled;
    private bool canShowArrow;

    public bool grabbed;
    public bool isClosed = false;

    public bool CantSetArrow = false;

    public void SetShowingArrow(bool flag)
    {
        if (flag && CantSetArrow)
            return;

        canShowArrow = flag;
        arrowEnabled = flag;
    }

    private void Awake() => arrowEnabled = canShowArrow = grabbed = false;

    private void Start()
    {
        interactableObjScript = GetComponent<VRTK_InteractableObject>();
        interactableObjScript.InteractableObjectGrabbed += Grabbed;
        interactableObjScript.InteractableObjectUngrabbed += UnGrabbed;

        interactableObjScript.InteractableObjectEnteredSnapDropZone += EnteredSnapDropZone;
        interactableObjScript.InteractableObjectExitedSnapDropZone += UnGrabbed;

        if (Arrow != null)
            arr = Instantiate(Arrow, transform.position + Vector3.up, Quaternion.identity);
    }

    private void Grabbed(object sender, InteractableObjectEventArgs e)
    {
        arrowEnabled = false;
        grabbed = true;
    }

    private void UnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        arrowEnabled = (canShowArrow) ? true : false;
        grabbed = false;
    }

    private void EnteredSnapDropZone(object sender, InteractableObjectEventArgs e)
    {
        canShowArrow = false;
        Destroy(arr);
    }

    public void SetArrowAgain()
    {
        canShowArrow = true;
        arrowEnabled = true;
    }

    void Update()
    {
        if (arr)
        {
            if (arrowEnabled && !arr.activeSelf)
                arr.SetActive(true);
            else
            if (!arrowEnabled && arr.activeSelf)
                arr.SetActive(false);

            if (isClosed)
                arr.SetActive(false);

            if (arrowEnabled && !isClosed)
            {
                Collider col = GetComponent<Collider>();
                var point = col.bounds.center;
                var h = col.bounds.size / 2f;
                arr.transform.Rotate(new Vector3(0, 1, 0));
                arr.transform.position = point + new Vector3(0, h.y, 0) + new Vector3(0, .5f, 0);
            }
        }    
    }
}
