using UnityEngine;
using VRTK;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    GameObject CameraVR;

    [SerializeField]
    VRTK_ControllerEvents left;

    [SerializeField]
    VRTK_ControllerEvents right;

    public float MaxY = 6.2f, MinY = 4.7f;
    public bool ChangeLocation;
    private bool pressed = false;
    private float _distance = 2.5f;

    void Start()
    {
        left.TouchpadPressed += new ControllerInteractionEventHandler(SetPressed);
        right.TouchpadPressed += new ControllerInteractionEventHandler(SetPressed);
        right.TouchpadReleased += new ControllerInteractionEventHandler(SetPressed);
        left.TouchpadReleased += new ControllerInteractionEventHandler(SetPressed);


        MinY = CameraVR.transform.position.y + 0.4f;
        MaxY = CameraVR.transform.position.y + 1.2f;

        MenuLocation();
    }

    public void MenuLocation()
    {
        this.GetComponent<RectTransform>().position = new Vector3(CameraVR.transform.position.x,
                CameraVR.transform.position.y, CameraVR.transform.position.z) + CameraVR.transform.forward * _distance;

        if (CameraVR.transform.position.y + CameraVR.transform.forward.y * _distance < MinY)
        {
            this.GetComponent<RectTransform>().position = new Vector3(this.GetComponent<RectTransform>().position.x,
                MinY, this.GetComponent<RectTransform>().position.z);
        }
        else if (CameraVR.transform.position.y + CameraVR.transform.forward.y * _distance > MaxY)
        {
            this.GetComponent<RectTransform>().position = new Vector3(this.GetComponent<RectTransform>().position.x,
                MaxY, this.GetComponent<RectTransform>().position.z);
        }

        this.GetComponent<RectTransform>().eulerAngles = new Vector3(0, CameraVR.transform.rotation.eulerAngles.y, 0);
    }

    private void Update()
    {
        if (ChangeLocation)
            MenuLocation();
    }

    void SetPressed(object sender, ControllerInteractionEventArgs e) => pressed = !pressed;
}
