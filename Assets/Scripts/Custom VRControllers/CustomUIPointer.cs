using UnityEngine;
using VRTK;

public class CustomUIPointer : MonoBehaviour
{
    [SerializeField]
    private VRTK_ControllerEvents LeftController;
    [SerializeField]
    private VRTK_ControllerEvents RightController;

    private void Awake()
    {
        LeftController.GetComponent<VRTK_UIPointer>().UIPointerElementEnter += LeftUIPointer_UIPointerElementEnter;
        RightController.GetComponent<VRTK_UIPointer>().UIPointerElementEnter += RightUIPointer_UIPointerElementEnter;

        LeftController.GetComponent<VRTK_UIPointer>().UIPointerElementExit += LeftCustomUIPointer_UIPointerElementExit; ;
        RightController.GetComponent<VRTK_UIPointer>().UIPointerElementExit += RightCustomUIPointer_UIPointerElementExit;
    }

    private void LeftCustomUIPointer_UIPointerElementExit(object sender, UIPointerEventArgs e)
    {
        LeftController.GetComponent<VRTK_StraightPointerRenderer>().tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive;
        LeftController.GetComponent<VRTK_StraightPointerRenderer>().cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive;

        if (e.previousTarget != null)
            LeftController.GetComponent<VRTK_Pointer>().interactWithObjects = false;
    }

    private void LeftUIPointer_UIPointerElementEnter(object sender, UIPointerEventArgs e)
    {
        LeftController.GetComponent<VRTK_StraightPointerRenderer>().tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;
        LeftController.GetComponent<VRTK_StraightPointerRenderer>().cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;

        if (e.raycastResult.gameObject != null)
            if (e.raycastResult.module.tag == "Posters")
                LeftController.GetComponent<VRTK_Pointer>().interactWithObjects = true;
    }

    private void RightCustomUIPointer_UIPointerElementExit(object sender, UIPointerEventArgs e)
    {
        RightController.GetComponent<VRTK_StraightPointerRenderer>().tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive;
        RightController.GetComponent<VRTK_StraightPointerRenderer>().cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive;

        if (e.previousTarget != null)
            RightController.GetComponent<VRTK_Pointer>().interactWithObjects = false;
    }

    private void RightUIPointer_UIPointerElementEnter(object sender, UIPointerEventArgs e)
    {
        RightController.GetComponent<VRTK_StraightPointerRenderer>().tracerVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;
        RightController.GetComponent<VRTK_StraightPointerRenderer>().cursorVisibility = VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn;

        if (e.raycastResult.gameObject != null)
            if (e.raycastResult.module.tag == "Posters")
                RightController.GetComponent<VRTK_Pointer>().interactWithObjects = true;
    }
}
