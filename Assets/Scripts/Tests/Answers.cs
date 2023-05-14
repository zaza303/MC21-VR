using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Answers : MonoBehaviour
{
    [SerializeField]
    GameObject RightPanel;

    [SerializeField]
    GameObject WrongPanel;

    public void Right()
    {
        RightPanel.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition;
        RightPanel.GetComponent<RectTransform>().eulerAngles = this.GetComponent<RectTransform>().eulerAngles;

        RightPanel.SetActive(true);    

        var buttons = GetComponentsInChildren<Button>();
        foreach (var but in buttons)
        {
            but.interactable = false;
        }
        StartCoroutine(Timer());

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("Location 1");
        }
    }

    public void Mistake()
    {
        WrongPanel.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition;
        WrongPanel.GetComponent<RectTransform>().eulerAngles = this.GetComponent<RectTransform>().eulerAngles;

        WrongPanel.SetActive(true);

        var buttons = GetComponentsInChildren<Button>();
        foreach (var but in buttons)
        {
            but.interactable = false;
        }

        StartCoroutine(Timer());

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(3);
            WrongPanel.SetActive(false);
            foreach (var but in buttons)
            {
                but.interactable = true;
            }
        }
    }
}
