using UnityEngine;
using VRTK;

public class DropZoneBase : MonoBehaviour
{
    [SerializeField]
    private VRTK_SnapDropZone _SnapDropZone;
    [SerializeField]
    private GameObject _gObject;
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Collider _GOCollider;
    [SerializeField]
    private bool _haveAdditional;

    public bool IsPortable;

    public bool IsColliderHighlightActive = false;

    public VRTK_SnapDropZone SnapDropZone { get => _SnapDropZone; set => _SnapDropZone = value; }
    public GameObject GObject { get => _gObject; set => _gObject = value; }
    public GameObject Prefab { get => _prefab; set => _prefab = value; }
    public Collider GOCollider { get => _GOCollider; set => _GOCollider = value; }
    public bool HaveAdditional { get => _haveAdditional; set => _haveAdditional = value; }
    public GameObject CurrentSnappedObject { get; private set; }
    private void OnEnable()
    {
        _SnapDropZone = GetComponent<VRTK_SnapDropZone>();
        _gObject = gameObject;
        _prefab = _SnapDropZone.highlightObjectPrefab;
        _GOCollider = GetComponent<Collider>();
        _haveAdditional = GetComponent<NotOrderList>();

        _SnapDropZone.ObjectSnappedToDropZone += SnappedToDropZone;
    }

    public void SnappedToDropZone(object sender, SnapDropZoneEventArgs e)
    {
        CurrentSnappedObject = e.snappedObject;
    }

    public void SetCurrentSnappedObject(GameObject snappedObject)
    {
        CurrentSnappedObject = snappedObject;
    }

    public void SetColliderHighlightActive(bool active)
    {
        _GOCollider.enabled = active;
        SnapDropZone.highlightAlwaysActive = active;
        IsColliderHighlightActive = active;
    }
}
