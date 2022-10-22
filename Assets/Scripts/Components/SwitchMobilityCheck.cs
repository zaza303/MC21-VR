using UnityEngine;
using VRTK;

public class SwitchMobilityCheck : DraggedObject
{
    void Update()
    {
        if (canOpen)
        {
            if (!swithPrefab.GetComponent<Collider>().enabled)
            {
                swithPrefab.GetComponent<Collider>().enabled = true;
                swithPrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                DropZoneBase.SnapDropZone.enabled = false;
            }

            swithPrefab.GetComponent<VRTK_InteractableObject>().isGrabbable = false;

            if (sign > 0)
            {
                if (time < 30)
                    swithPrefab.transform.Rotate(new Vector3(0, sign * rotationSpeed * Time.deltaTime, 0));
                else
                    sign = -1;
            }
            else
            {
                if (time < 60)
                    swithPrefab.transform.Rotate(new Vector3(0, sign * rotationSpeed * Time.deltaTime, 0));
                else
                    Done = true;
            }

            time += rotationSpeed * Time.deltaTime;
        }
        else if (!canOpen)
            swithPrefab.GetComponent<Collider>().enabled = false;
    }
}
