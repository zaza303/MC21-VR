using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{
[SerializeField] GameObject Text1;

void Start()
{
Text1.SetActive(false);
}
public void OnMouseOver()
{
Text1.SetActive(true);
}

public void OnMouseExit()
{
Text1.SetActive(false);
}
}