using UnityEngine;

public class Hole : MonoBehaviour
{
    public float TimeToHole;
    public Color CurrentColor { get; private set; }
    public bool WasDrill;
    private float maxTimeToHole = 1;
    private void Awake()
    {
        SetMaxTime();
        WasDrill = false;
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
        CurrentColor = color;
    }

    public void SetMaxTime()
    {
        TimeToHole = maxTimeToHole;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
