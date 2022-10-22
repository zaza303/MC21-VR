using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UpdateResults : MonoBehaviour
{
    private string reportDirectoryName = "Report";
    private string reportFileName = "report.csv";

    [SerializeField]
    GameObject Prefab;

    [SerializeField]
    GameObject Content;

    public void SetResults()
    {
        string dir = GetDirectoryPath();
        if (Directory.Exists(dir))
        {
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }

            string[] dataInFile = File.ReadAllLines(GetFilePath(), Encoding.UTF8);

            for (int i = 0; i < dataInFile.Length; i++)
            {
                var temp = dataInFile[i].Split(';');
                var prefab = GameObject.Instantiate(Prefab.gameObject, Content.transform);

                var i1 = 0;
                foreach (Transform child in prefab.transform)
                {
                    foreach (Transform ch in child.transform)
                    {
                        ch.gameObject.GetComponent<Text>().text = temp[i1];
                    }

                    i1++;
                }
            }
        }
    }

    private Transform[] GetChilds(Transform parent)
    {
        List<Transform> ret = new List<Transform>();
        foreach (Transform child in parent) ret.Add(child);
        return ret.ToArray();
    }

    private string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }
    private string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }
}
