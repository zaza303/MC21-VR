using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class createDetail : MonoBehaviour {
    [SerializeField] private Vector3 spawnPoint; //точка спавна объектов
    public float spawnOffsetX = .5f;
    [SerializeField] private Vector3 spawnPointInstr; //точка спавна инструментов
    [SerializeField] private GameObject[] Details; // массив деталей
    [SerializeField] private GameObject[] Tools; //массив инструментов для работы с деталями
    private int[] randomSpawnIndexes;


    // Start is called before the first frame update
    void Start()
    {
        CreateRandomSpawnIndexes();
        for (int i = 0; i < 5 && i < randomSpawnIndexes.Length; i++) {
            GameObject objectToSpawn = Details[randomSpawnIndexes[i]];
            CreateObject(objectToSpawn, i);
            CreateTools(i);
        }
    }

    private void CreateRandomSpawnIndexes()
    {
        var maxPairsOfDetailsAndTools = Mathf.Min(Details.Length, Tools.Length);
        System.Random random = new System.Random();
        int[] values = Enumerable.Range(0, maxPairsOfDetailsAndTools)
            .OrderBy((a) => random.Next(-1, 2))
            .ToArray();

        randomSpawnIndexes = values;
    }

    // функция для создания на сцене этой самой детальки
    void CreateObject(GameObject objectToSpawn, int i)
    {
        Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);
        var text = CreateText(objectToSpawn.transform);
        objectToSpawn.GetComponent<ShowText>().FloatingText = text.gameObject;
        text.text = objectToSpawn.gameObject.name;
        var offset = 0.3f;
        var offsetOfModels = Mathf.Min(5, randomSpawnIndexes.Length);
        var offsetAbsolute = new Vector3(objectToSpawn.transform.position.x + offset * (i - offsetOfModels), objectToSpawn.transform.position.y + offset, objectToSpawn.transform.position.z);
        text.gameObject.transform.Translate(offsetAbsolute);

        spawnPoint.x += spawnOffsetX;
    }

    static Text CreateText(Transform parent)
    {
        var canvasGameObject = new GameObject();
        canvasGameObject.transform.SetParent(parent, false);
        var canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.transform.localScale = Vector3.one / 180;

        var textGameObject = new GameObject();
        textGameObject.transform.SetParent(canvasGameObject.transform, false);
        var text = textGameObject.AddComponent<Text>();
        Font font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = font;
        text.gameObject.AddComponent<FolowPlayerUI>();
        text.gameObject.GetComponent<FolowPlayerUI>().SetCamera();

        return text;
    }

    void CreateTools(int i)
    {
        int randomTool = randomSpawnIndexes[i];                       // тут присваиваем счетчику значение от 0 до количества интрументов в массиве инструментов. Это для того, что бы у нас брался не первый.. 
                                                                      // записываем, что инструмент уже использовали
        Instantiate(Tools[randomTool], spawnPointInstr, Quaternion.identity);       // создаем этот инструмент
        spawnPointInstr.x -= spawnOffsetX;                                        // сдвигаем спавн объекта на 3 еденицы 

    }
}
