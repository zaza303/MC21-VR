using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    [SerializeField] private GameObject HelpTextCanvas;

    private Animator animHelpPanel;
    private RotateForCamera rotateScript;

    private void Awake()
    {
        animHelpPanel = HelpTextCanvas.GetComponent<Animator>();
        rotateScript = HelpTextCanvas.GetComponent<RotateForCamera>();
    }

    public void Right()
    {
        animHelpPanel.SetTrigger("open");
        var text = HelpTextCanvas.GetComponentInChildren<Text>();
        text.text = "+1";
        text.color = Color.green;

        if (rotateScript != null)
            rotateScript.Rotate();
    }

    public void Wrong()
    {
        animHelpPanel.SetTrigger("open");
        var text = HelpTextCanvas.GetComponentInChildren<Text>();
        text.text = "-1";

        text.color = Color.red;

        if (rotateScript != null)
            rotateScript.Rotate();
    }

    public void OpenHelp()
    {
        animHelpPanel.SetBool("open", true);
        animHelpPanel.SetBool("close", false);

        if (rotateScript != null)
            rotateScript.Rotate();
    }

    public void CloseHelp()
    {
        animHelpPanel.SetBool("close", true);
        animHelpPanel.SetBool("open", false);
    }
}
