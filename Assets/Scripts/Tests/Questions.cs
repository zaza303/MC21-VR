using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questions : MonoBehaviour
{
    [SerializeField]
    GameObject[] QuestionsCanvas;

    private int CountOfQuestions;
    private int i = 0;
    private int _countOfRightAnsw = 0;
    private List<int> selectedValues = new List<int>();

    private void Start()
    {
        CountOfQuestions = QuestionsCanvas.Length;
        QuestionsCanvas[i].SetActive(true);
    }

    public void Answer(bool Flag)
    {
        var image = QuestionsCanvas[i].GetComponentInChildren<Image>();
        var tempColor = image.color;
        image.color = (Flag) ? Color.green : Color.red;

        i++;

        var buttons = QuestionsCanvas[i - 1].GetComponentsInChildren<Button>();
        foreach (var but in buttons)
        {
            but.interactable = false;
        }
        StartCoroutine(Timer());

        IEnumerator Timer()
        {
            if (Flag == true)
                _countOfRightAnsw++;
            yield return new WaitForSeconds(2);
            if (i < CountOfQuestions)
            {
                image.color = tempColor;

                QuestionsCanvas[i - 1].SetActive(false);
                QuestionsCanvas[i].SetActive(true);

                var newbuttons = QuestionsCanvas[i].GetComponentsInChildren<Button>();
                foreach (var but in newbuttons)
                {
                    but.interactable = true;
                }

                QuestionsCanvas[i].GetComponent<Canvas>().enabled = true;
            }
        }
    }

    public void SaveResults()
    {
        CSVManager.AppendToReport(PlayerPrefs.GetString("GameName"), SetScoreString(), "Тестирование");
    }

    public void SetVariant(int num)
    {
        var buttons = QuestionsCanvas[i].GetComponentsInChildren<Button>();
        var tempColor = buttons[buttons.Length - 1].GetComponent<Image>().color;

        if (selectedValues.Contains(num))
        {
            selectedValues.Remove(num);
            buttons[num - 1].GetComponent<Image>().color = tempColor;
        }
        else
        {
            selectedValues.Add(num);
            buttons[num - 1].GetComponent<Image>().color = Color.cyan;
        }
    }

    public void Check(string rightValues)
    {
        string result = "";
        selectedValues.Sort();
        foreach (var item in selectedValues)
        {
            result += item.ToString();
        }
        if (result.Equals(rightValues))
            Answer(true);
        else
            Answer(false);
    }

    public string SetScoreString()
    {
        string finalString;
        finalString = _countOfRightAnsw.ToString() + " из " + (CountOfQuestions - 1);
        return finalString;
    }
}
