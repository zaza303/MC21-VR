using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createDetail : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private GameObject[] tools;
    // Start is called before the first frame update
    void Start()
    {
       
       GameObject objectToSpawn =  SelectItem();
       CreateObject(objectToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject SelectItem()
    {
        
        var index = Random.Range(0,tools.Length);
        return tools[index];
    }
    void CreateObject(GameObject objectToSpawn)
    {

        Instantiate(objectToSpawn,spawnPoint,Quaternion.identity);
    }
}
