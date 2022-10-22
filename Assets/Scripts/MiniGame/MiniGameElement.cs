using UnityEngine;
using VRTK;

public class MiniGameElement : MonoBehaviour
{
    public int ZoneNumber;
    public bool Done;
    private VRTK_InteractableObject interactableObject;
    private Rigidbody rigidbody;
    private VRTK_InteractObjectHighlighter objectHighlighter;
    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        objectHighlighter = GetComponent<VRTK_InteractObjectHighlighter>();
        rigidbody = GetComponent<Rigidbody>();
        Done = false;
    }

    public void Over()
    {
        Done = true;
        Destroy(objectHighlighter);
        interactableObject.isGrabbable = false;
    }
}
