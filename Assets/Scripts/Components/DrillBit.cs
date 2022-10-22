using UnityEngine;

public class DrillBit : MonoBehaviour
{
    public delegate void OnColliderTouch(GameObject gameObject);
    public event OnColliderTouch TouchObjectEvent;
    public event OnColliderTouch UnTouchObjectEvent;
    private void OnTriggerEnter(Collider other) => TouchObjectEvent(other.gameObject);
    private void OnTriggerExit(Collider other) => UnTouchObjectEvent(other.gameObject);
}
