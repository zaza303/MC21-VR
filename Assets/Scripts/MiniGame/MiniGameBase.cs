using System;
using System.Collections;
using UnityEngine;

public class MiniGameBase : MonoBehaviour
{
    [SerializeField]
    private int ZoneNumber;
    [SerializeField]
    private MiniGameManager miniGameManager;
    private Color defaultColor;
    
    private void Awake()
    {
        defaultColor = GetComponent<Renderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        var mg = other.GetComponent<MiniGameElement>();
        if (mg != null && !mg.Done)
        {
            mg.Over();

            if (ZoneNumber == mg.ZoneNumber)
            {
                miniGameManager.SetScore();
                StartCoroutine(Right());
            }
            else
                StartCoroutine(Wrong());
        }
    }

    private IEnumerator Wrong()
    {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(.5f);
        GetComponent<Renderer>().material.color = defaultColor;
    }

    private IEnumerator Right()
    {
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(.5f);
        GetComponent<Renderer>().material.color = defaultColor;
    }
}
