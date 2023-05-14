using UnityEngine;

public class HelpPanel : MonoBehaviour
{

    [SerializeField] private GameObject HelpTextCanvas;

    private Animator animHelpPanel;

    private void Start()
    {
        animHelpPanel = HelpTextCanvas.GetComponent<Animator>();
    }

    public void OpenHelp()
    {
        animHelpPanel.SetBool("open", true);
        animHelpPanel.SetBool("close", false);
    }

    public void CloseHelp()
    {
        animHelpPanel.SetBool("close", true);
        animHelpPanel.SetBool("open", false);
    }
}
