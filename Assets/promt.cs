using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class promt : MonoBehaviour
{

    private VRTK_InteractableObject interactableObject;
    private AudioSource audioSource;
    private bool played = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("123");

        interactableObject = GetComponent<VRTK_InteractableObject>();
        audioSource = GetComponent<AudioSource>();

        interactableObject.InteractableObjectGrabbed += OnObjectGrable;
    }

  
    private void OnObjectGrable(object sender, InteractableObjectEventArgs e)
    {

        Debug.Log("grabbed");
        if (!played)
        {
            audioSource.Play();
            played = true;
        }
    }
}
