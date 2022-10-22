using UnityEngine;
using System.IO;
using System.Text;

public static class CSVManager
{
    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ";";
    private static string[] reportHeaders = new string[4] {
        "Имя", "Режим", "Количество очков", "Дата"};

    #region Interactions

    public static void AppendToReport(string Name, string Score, string ScenarioName)
    {
        VerifyDirectory();
        VerifyFile();
        
        string finalString = "";

        for (int i = 0; i < reportHeaders.Length - 1; i++)
        {
            if (i == 0)
                finalString += Name;
            else if (i == 1)
                finalString += ScenarioName;
            else if (i == 2)
                finalString += Score;

            if (finalString != "")
                finalString += reportSeparator;
        }

        StreamWriter sw = new StreamWriter(GetFilePath(), true, Encoding.UTF8);
        finalString += GetTimeStamp();
        sw.WriteLine(finalString);
        sw.Close();
    }

    public static void CreateReport()
    {
        VerifyDirectory();
        StreamWriter sw = new StreamWriter(GetFilePath(), true, Encoding.UTF8);
        string finalString = "";
        for (int i = 0; i < reportHeaders.Length; i++)
        {
            if (finalString != "")
                finalString += reportSeparator;

            finalString += reportHeaders[i];
        }

        sw.WriteLine(finalString);
        sw.Close();
    }

    #endregion


    #region Operations

    static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateReport();
        }
    }

    #endregion


    #region Queries

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }

    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }

    static string GetTimeStamp()
    {
        return System.DateTime.Now.ToString();
    }

    #endregion

}
