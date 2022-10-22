using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject InfoPanel;
    [SerializeField]
    private List<MiniGameElement> miniGameElements;
    [SerializeField]
    private GameManager gameManager;

    private int CountOfItems;

    private int Score;
    private void Awake()
    {
        Menu.SetActive(false);
        InfoPanel.SetActive(true);
        Score = 0;
        ScoreText.text = Score.ToString();
        CountOfItems = miniGameElements.Count;
    }

    public void SetScore()
    {
        Score++;
        ScoreText.text = Score.ToString();
    }

    private void Update()
    {
        if (!Menu.activeSelf)
            GameOver();
    }

    private void GameOver()
    {
        if (miniGameElements.TrueForAll(x => x.Done))
        {
            gameManager.MenuPerssedFalse();
            Menu.SetActive(true);
            InfoPanel.SetActive(false);
            CSVManager.AppendToReport(PlayerPrefs.GetString("GameName"), SetScoreString(), "Мини-игра");
        }
    }

    public string SetScoreString()
    {
        string finalString;
        finalString = Score.ToString() + " из " + CountOfItems;
        return finalString;
    }
}
