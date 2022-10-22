using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private GetJson getJsonScript;
    [SerializeField]
    private Text DescriptionText;
    [SerializeField]
    private TrainingDropManager dropManager;
    private string currentKey;
    private void Awake()
    {
        getJsonScript = GetComponent<GetJson>();
        currentKey = "";
    }

    public void SetMode(int number)
    {
        PlayerPrefs.SetInt("Mode", number);
        if (number > 1)
            SceneManager.LoadScene("MainScene");
    }

    public void GetScenarioOne()
    {
        var nameOfScenario = "scenario1.txt";
        PlayerPrefs.SetString("Scenario", nameOfScenario);
        SceneManager.LoadScene("MainScene");
    }

    public void GetScenarioTwo()
    {
        var nameOfScenario = "scenario2.txt";
        PlayerPrefs.SetString("Scenario", nameOfScenario);
        SceneManager.LoadScene("MainScene2");
    }

    public void SetSubMode(string nameOfScenario)
    {
        PlayerPrefs.SetString("Scenario", nameOfScenario);
        SceneManager.LoadScene("MainScene");
    }

    public void ChangePanel(string keys)
    {
        var nums = keys.Split(' ');
        if (currentKey == "")
        {
            currentKey = nums[0];
            Pair item = getJsonScript.Items.Find(x => x.key == currentKey);
            DescriptionText.text = item.value;
            dropManager.SetSound(currentKey);
        }
        else
        {
            for (int i = 0; i < nums.Length; i++)
            {
                if (currentKey == nums[i])
                {
                    if (i < nums.Length - 1)
                    {
                        currentKey = nums[++i];
                        if (currentKey != "")
                        {
                            Pair item = getJsonScript.Items.Find(x => x.key == currentKey);
                            DescriptionText.text = item.value;
                            dropManager.SetSound(currentKey);
                        }
                        break;
                    }
                    else
                    {
                        GetComponent<GameManager>().StartButton();
                        break;
                    }
                }
            }
        }
    }

    public void ExitButton() => Application.Quit();

    public void MainMenu() => SceneManager.LoadScene("Menu");
}
