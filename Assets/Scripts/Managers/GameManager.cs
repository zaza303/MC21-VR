using UnityEngine;
using UnityEngine.UI;
using VRTK;

public enum GameMode { TrainingMode = 0, CheckMode = 1, TestMode = 2, MiniGame = 3 }
public class GameManager : MonoBehaviour
{
    public GameMode CurrentMode;

    [SerializeField]
    private TrainingDropManager trainingDropManager;
    [SerializeField]
    private CheckDropManager checkDropManager;
    [SerializeField]
    private GameObject questionsCanvas;
    [SerializeField]
    private GameObject miniGame;
    [SerializeField]
    private Transform[] Points;
    [SerializeField]
    private Transform RoomVR;
    [SerializeField]
    private VRTK_ControllerEvents LeftController;
    [SerializeField]
    private VRTK_ControllerEvents RightController;
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private GameObject HelpPanel;
    [SerializeField]
    private GameObject StartButtonGO;
    [SerializeField]
    private GameObject ScorePanel;
    [SerializeField]
    private GameObject ExamMusic;
    [SerializeField]
    private GameObject MiniGameMusic;
    [SerializeField]
    private GameObject TestMusic;

    public static int Score;
    private static bool setScore = false;
    private int maxScoreFirst = 62;
    private int maxScoreSecond = 100;

    private void Awake()
    {
        CurrentMode = (GameMode)PlayerPrefs.GetInt("Mode");
        trainingDropManager.gameObject.SetActive(false);
        checkDropManager.gameObject.SetActive(false);
        questionsCanvas.SetActive(false);
        miniGame.SetActive(false);
        Menu.SetActive(false);
        ScorePanel.SetActive(false);
        HelpPanel.SetActive(false);
        Score = 0;
        ScoreText.text = Score.ToString();

        LeftController.ButtonTwoPressed += new ControllerInteractionEventHandler(MenuPressed);
        RightController.ButtonTwoPressed += new ControllerInteractionEventHandler(MenuPressed);

        switch (CurrentMode)
        {
            case GameMode.TrainingMode:
                Settings(trainingDropManager.gameObject, 0, VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive, true);
                HelpPanel.SetActive(true);
                ExamMusic.SetActive(false);
                MiniGameMusic.SetActive(false);
                TestMusic.SetActive(false);
                break;
            case GameMode.CheckMode:
                Settings(checkDropManager.gameObject, 0, VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive, true);
                ScorePanel.SetActive(true);
                ExamMusic.SetActive(true);
                MiniGameMusic.SetActive(false);
                TestMusic.SetActive(false);
                break;
            case GameMode.TestMode:
                Settings(questionsCanvas, 1, VRTK_BasePointerRenderer.VisibilityStates.AlwaysOn, false);
                ExamMusic.SetActive(false);
                MiniGameMusic.SetActive(false);
                TestMusic.SetActive(true);
                break;
            case GameMode.MiniGame:
                Settings(miniGame, 2, VRTK_BasePointerRenderer.VisibilityStates.OnWhenActive, true);
                ExamMusic.SetActive(false);
                MiniGameMusic.SetActive(true);
                TestMusic.SetActive(false);
                break;
            default:
                break;
        }

        void Settings(GameObject objToActive, int numPosition, VRTK_BasePointerRenderer.VisibilityStates state, bool enableTeleport)
        {
            objToActive.SetActive(true);
            RoomVR.position = Points[numPosition].position;
            RoomVR.localRotation = Points[numPosition].localRotation;

            LeftController.GetComponent<VRTK_StraightPointerRenderer>().tracerVisibility = state;
            LeftController.GetComponent<VRTK_StraightPointerRenderer>().cursorVisibility = state;
            RightController.GetComponent<VRTK_StraightPointerRenderer>().tracerVisibility = state;
            RightController.GetComponent<VRTK_StraightPointerRenderer>().cursorVisibility = state;

            LeftController.GetComponent<VRTK_Pointer>().enableTeleport = enableTeleport;
            RightController.GetComponent<VRTK_Pointer>().enableTeleport = enableTeleport;
        }
    }

    public static void SetScore(int addScore)
    {
        Score += addScore;
        var scenario = PlayerPrefs.GetString("Scenario");
        if (Score < 0)
            Score = 0;
        if (scenario == "scenario2.txt")
        {
            if (Score > 100)
                Score = 100;
        }    
        else
        {
            if (Score > 62)
                Score = 62;
        }
       
        setScore = true;
    }

    private void Update()
    {
        if (setScore)
        {
            ScoreText.text = Score.ToString();
            setScore = false;
        }
    }

    private void MenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (Menu.activeSelf == false)
        {
            Menu.SetActive(true);
            Menu.GetComponent<MenuPanel>().MenuLocation();
        }
        else
            Menu.SetActive(false);
    }

    public void MenuPerssedFalse()
    {
        LeftController.ButtonTwoPressed -= new ControllerInteractionEventHandler(MenuPressed);
        RightController.ButtonTwoPressed -= new ControllerInteractionEventHandler(MenuPressed);
    }

    public void StartButton()
    {
        trainingDropManager.StartTraining();
        StartButtonGO.SetActive(false);
    }

    public void SetResult()
    {
        string finalString;
        var scenario = PlayerPrefs.GetString("Scenario");
        if (scenario == "scenario2.txt")
        {
            finalString = Score.ToString() + " из " + maxScoreSecond;
            CSVManager.AppendToReport(PlayerPrefs.GetString("GameName"), finalString, "Экзамен по монтажу элементов сборочного приспособления");
        }
        else
        {
            finalString = Score.ToString() + " из " + maxScoreFirst;
            CSVManager.AppendToReport(PlayerPrefs.GetString("GameName"), finalString, "Экзамен по сборке сборочной единицы «панель»");
        }
    }

    public void AutoSnap()
    {
        switch (CurrentMode)
        {
            case GameMode.TrainingMode:
                trainingDropManager.AutoSnap();
                break;
            case GameMode.CheckMode:
                checkDropManager.AutoSnap();
                break;
            default:
                break;
        }
    }

    public void test()
    {
        Debug.Log("работает");
    }
}
