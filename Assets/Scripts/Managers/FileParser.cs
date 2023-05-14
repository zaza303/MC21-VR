using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileParser : MonoBehaviour
{
    public List<string> ListOfNames = new List<string> { };
    public string reportFileName;// = "scenario2.txt";
    protected virtual void Awake()
    {
        SetScenarioName(PlayerPrefs.GetString("Scenario"));
        ReadScenario();
    }

    public void SetScenarioName(string newName)
    {
        if (newName != "")
            reportFileName = newName;
    }

    private void ReadScenario()
    {
        if (VerifyDirectoryNFile())
        {
            var dir = GetFilePath();
            foreach (string line in File.ReadLines(dir))
                ListOfNames.Add(line);
        }
    }

    private bool VerifyDirectoryNFile()
    {
        string dir = GetDirectoryPath();
        string file = GetFilePath();
        if (!Directory.Exists(dir))
            return false;
        if (!File.Exists(file))
            return false;
        return true;
    }

    private string GetDirectoryPath() => Application.dataPath + "/StreamingAssets";
    private string GetFilePath() => GetDirectoryPath() + "/" + reportFileName;
}
