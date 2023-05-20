using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour {
    public GameObject FloatingText;

    void Start()
    {
        FloatingText.SetActive(false);
    }
    public void OnMouseOver()
    {
        FloatingText.SetActive(true);
    }

    public void OnMouseExit()
    {
        FloatingText.SetActive(false);
    }
}