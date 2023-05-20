using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usingWithDetail : MonoBehaviour
{
    [SerializeField] public string[] details; 
    [SerializeField] private GameObject banner;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        bool good=false;
        foreach(string s in details)
        {
            if(s==other.gameObject.tag){ good=true;break;}
            else good=false;
        }
        if(good) GoodConnection();
        else BadConnection();
    }
    void GoodConnection()
    {
        Debug.Log("Good");
    }
    void BadConnection(){
    Debug.Log("Bad");
    }
}
