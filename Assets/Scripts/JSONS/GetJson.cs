using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GetJson : MonoBehaviour
{
    public List<Pair> Items = new List<Pair>();

    private void Awake() => LoadItem(getPath());

    public void LoadItem(string path)
    {
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path, Encoding.UTF8);
            Pair[] myItem = JsonHelper.FromJson<Pair>(jsonString);
            Items.Clear();

            foreach (Pair item in myItem)
            {
                Items.Add(item);
            }
        }
        else
            Debug.LogError("Json file not found");
    }

    private string getPath()
    {
        return Application.dataPath + "/StreamingAssets/" + "description" + ".json";
    }
}

[Serializable]
public class Pair
{
    public string key;
    public string value;
}
