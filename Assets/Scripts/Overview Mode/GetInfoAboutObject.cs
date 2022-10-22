using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GetInfoAboutObject : MonoBehaviour
{
    [SerializeField]
    GameObject InfoPanel;

    [SerializeField]
    GameObject CameraVR;

    [SerializeField]
    VRTK_ControllerEvents left;

    [SerializeField]
    VRTK_ControllerEvents right;

    private bool isTrigger = false;
    private bool panelIsOn = false;

    void Start()
    {
        left.TriggerPressed += new ControllerInteractionEventHandler(InfoPressed);
        right.TriggerPressed += new ControllerInteractionEventHandler(InfoPressed);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "[VRTK][AUTOGEN][RightControllerScriptAlias][StraightPointerRenderer_Cursor]"
            || col.gameObject.name == "[VRTK][AUTOGEN][LeftControllerScriptAlias][StraightPointerRenderer_Cursor]")
        {
            isTrigger = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "[VRTK][AUTOGEN][RightControllerScriptAlias][StraightPointerRenderer_Cursor]"
            || col.gameObject.name == "[VRTK][AUTOGEN][LeftControllerScriptAlias][StraightPointerRenderer_Cursor]")
        {
            isTrigger = false;
        }
    }

    void InfoPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (isTrigger)
        {
            if (panelIsOn == false)
            {
                InfoPanel.GetComponent<RectTransform>().eulerAngles = new Vector3(0, CameraVR.transform.rotation.eulerAngles.y, 0);

                this.GetComponent<Light>().color = Color.green;
                this.GetComponent<MeshRenderer>().materials[0].color = Color.green;

                InfoPanel.GetComponent<Animator>().Play("Info in");

                panelIsOn = true;
            }
            else
            {
                this.GetComponent<Light>().color = Color.red;
                this.GetComponent<MeshRenderer>().materials[0].color = Color.red;

                InfoPanel.GetComponent<Animator>().Play("Info out");

                panelIsOn = false;
            }
        }
    }
}
