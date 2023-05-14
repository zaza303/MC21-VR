using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ShowPoster : MonoBehaviour
{
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
        left.TriggerPressed += new ControllerInteractionEventHandler(PosterPressed);
        right.TriggerPressed += new ControllerInteractionEventHandler(PosterPressed);
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

    void PosterPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (isTrigger)
        {
            if (panelIsOn == false)
            {
                GetComponent<Transform>().eulerAngles = new Vector3(0, CameraVR.transform.rotation.eulerAngles.y, 0);
                GetComponent<Animator>().Play("Poster in");

                panelIsOn = true;
            }
            else
            {
                GetComponent<Animator>().Play("Poster out");
                panelIsOn = false;
            }
        }
    }
}
