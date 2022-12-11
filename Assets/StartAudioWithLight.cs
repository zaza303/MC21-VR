using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class StartAudioWithLight : MonoBehaviour
{
    public GameObject SpotLight;
    public GameObject Sound;
    public GameObject NextDesk;
    public GameObject Robot;
    public Transform[] WayPoints;

    private Light light;
    private AudioSource audio;
    private float audioLength;
    private Animator anim;
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        light = SpotLight.GetComponent<Light>();
        audio = Sound.GetComponent<AudioSource>();
        audioLength = audio.clip.length;
        anim = Robot.GetComponent<Animator>();

        initialScale = NextDesk.transform.localScale;
        NextDesk.transform.localScale = new Vector3(0, 0, 0);
        light.gameObject.SetActive(false);
        //NextDesk.gameObject.SetActive(false); 
    }

    public void BClick()
    {
        if (!audio.isPlaying)
        {
            var robotAI = Robot.GetComponent<AI_Enemy>();
            robotAI.WayPoints = WayPoints;
            robotAI.StartWalking();

            Invoke("TurnOnLight", audioLength);
            audio.Play();
        }
        else {
            TurnOnLight();
            audio.Stop();
        }
        
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
    }

    void TurnOnLight()
    {
        light.gameObject.SetActive(true);
        NextDesk.transform.localScale = initialScale;
        //NextDesk.gameObject.SetActive(true);
    }
}
