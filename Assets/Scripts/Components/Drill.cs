using UnityEngine;
using VRTK;
public class Drill : MonoBehaviour
{
    [SerializeField]
    private GameObject bit;
    [SerializeField]
    private GameObject button;

    public bool IsDestroying;

    private AudioSource audioSource;
    private VRTK_InteractableObject linkedObject;
    private bool drillIsOn;
    private float speed;
    private float minSpeedValue = 0.5f;
    private float maxSpeedValue = 30;
    private float step = .1f;
    private bool isMakingHole;
    
    private Hole currentHole;

    public State CurrentState;
    public void Awake()
    {
        drillIsOn = false;
        isMakingHole = false;
        speed = minSpeedValue;
        audioSource = GetComponent<AudioSource>();
        CurrentState = State.Drill;
    }

    private void OnEnable()
    {
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectGrabbed += LinkedObject_InteractableObjectGrabbed;
            linkedObject.InteractableObjectUngrabbed += LinkedObject_InteractableObjectUngrabbed;
        }

        bit.GetComponent<DrillBit>().TouchObjectEvent += Drill_TouchObjectEvent;
        bit.GetComponent<DrillBit>().UnTouchObjectEvent += Drill_UnTouchObjectEvent; ;
    }

    private void LinkedObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        drillIsOn = false;
        audioSource.Stop();
    }

    private void LinkedObject_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        drillIsOn = true;
        audioSource.Play();
    }

    private void Drill_UnTouchObjectEvent(GameObject gameObject)
    {
        if (gameObject.tag.Equals("Hole"))
        {
           if (CurrentState == State.Drill && gameObject.GetComponent<Hole>().CurrentColor != Color.yellow && !IsDestroying)
           {
                isMakingHole = false;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                currentHole = null;
           }
           else if (CurrentState == State.Countersink && IsDestroying)
           {
                isMakingHole = false;
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                currentHole = null;
           }
        }
    }

    private void Drill_TouchObjectEvent(GameObject gameObject)
    {
        if (gameObject.tag.Equals("Hole"))
        {
               if (CurrentState == State.Drill && gameObject.GetComponent<Hole>().CurrentColor != Color.yellow && !IsDestroying)
               {
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                    var holeScript = gameObject.GetComponent<Hole>();
                    isMakingHole = true;
                    if (currentHole == null)
                        currentHole = holeScript;
               }
            else if (CurrentState == State.Countersink && IsDestroying)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                var holeScript = gameObject.GetComponent<Hole>();
                isMakingHole = true;
                if (currentHole == null)
                    currentHole = holeScript;
            }
        }
    }

    private void OnDisable()
    {
        if (linkedObject != null)
            linkedObject.InteractableObjectGrabbed -= LinkedObject_InteractableObjectGrabbed;
    }

    private void Update()
    {
        if (drillIsOn)
        {
            bit.transform.Rotate(new Vector3(-speed, 0, 0));
            if (speed < maxSpeedValue)
                speed += step;
        }
        else
            speed = minSpeedValue;

        if (isMakingHole && drillIsOn)
        {
            if (currentHole.TimeToHole > 0)
            {
                currentHole.TimeToHole -= Time.deltaTime;
            }
            else
            {
                if (IsDestroying && CurrentState == State.Countersink)
                {
                    Destroy(currentHole.gameObject);
                    currentHole = null;
                    isMakingHole = false;
                }
                else if (!IsDestroying && CurrentState == State.Drill)
                {
                    //Destroy(currentHole.gameObject);
                    currentHole.SetColor(Color.yellow);
                    currentHole.transform.localPosition += new Vector3(0.0051f, 0, 0);
                    currentHole = null;
                    isMakingHole = false;
                }
            }
        }
    }
}