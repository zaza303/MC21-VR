using UnityEngine;
using VRTK;

public class ChangeColorToSnap : MonoBehaviour
{
    private Material material;
    public Material standartMaterial;
    private Color color = Color.blue;
    private bool change;

    private void Awake()
    {
        change = false;
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            standartMaterial = renderer.material;
        }
    }

    private void Update()
    {
        //if (!change)
        //{
        //    material = standartMaterial;
        //    Color color = material.color;
        //    color.a = 0;
        //    material.color = color;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<VRTK_SnapDropZone>() != null)
        {
            if (other.gameObject.GetComponent<VRTK_SnapDropZone>().validObjectListPolicy.identifiers[0] == gameObject.tag && material != null)
            {
                Color c = GetComponent<VRTK_InteractObjectHighlighter>().touchHighlight;
                c.a = 0;
                GetComponent<VRTK_InteractObjectHighlighter>().touchHighlight = c;
                //material = standartMaterial;
                material.color = color;
                change = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<VRTK_SnapDropZone>() != null)
        {
            if (other.gameObject.GetComponent<VRTK_SnapDropZone>().validObjectListPolicy.identifiers[0] == gameObject.tag && material != null)
            {
                //material = standartMaterial;
                change = false;
            }
        }
    }
}
