using System.Linq;
using UnityEngine;
using VRTK;

public class AutoSnappingHelp : MonoBehaviour
{
    [SerializeField]
    private AutoSnapping autoSnapping;
    [SerializeField]
    private int Count;
    private VRTK_SnapDropZone snapDropZone;
    
    private void OnEnable()
    {
        snapDropZone = GetComponent<VRTK_SnapDropZone>();

        snapDropZone.ObjectSnappedToDropZone += SnapDropZone_ObjectSnappedToDropZone;
    }

    private void SnapDropZone_ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
    {
        autoSnapping.ObjectsToSnap.Add(e.snappedObject);

        for (int i = 0; i < Count - 1; i++)
        {
            if (autoSnapping.ObjectsToSnap.Where(x => x.tag == e.snappedObject.tag).Count() < 6)
            {
                var stringer = Instantiate(e.snappedObject);
                autoSnapping.ObjectsToSnap.Add(stringer);
            }
        }
    }
}
