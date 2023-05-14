using UnityEngine;

public class RotateForCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject CameraVR;

    public void Rotate() => GetComponent<RectTransform>().eulerAngles = new Vector3(0, CameraVR.transform.rotation.eulerAngles.y, 0);
}
