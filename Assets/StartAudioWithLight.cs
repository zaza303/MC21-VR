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
    public AudioClip SecondClip;

    private Light light;
    private AudioSource robotAudio;
    private AudioClip clip;
    private float audioLength;
    private Animator anim;
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        light = SpotLight.GetComponent<Light>();
        clip = Sound.GetComponent<AudioSource>().clip;
        robotAudio = Robot.GetComponent<AudioSource>();

        audioLength = clip.length;
        anim = Robot.GetComponent<Animator>();

        initialScale = NextDesk.transform.localScale;
        NextDesk.transform.localScale = new Vector3(0, 0, 0);
        light.gameObject.SetActive(false);
    }

    public void BClick()
    {
        if (!robotAudio.isPlaying)
        {
            var robotAI = Robot.GetComponent<AI_Enemy>();
            robotAI.WayPoints = WayPoints;
            robotAI.StartWalking();

            Invoke("TurnOnLight", audioLength);
            robotAudio.clip = clip;
            robotAudio.Play();
        }
        else {
            TurnOnLight();
            robotAudio.Stop();
        }
        
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
    }

    void TurnOnLight()
    {
        light.gameObject.SetActive(true);
        NextDesk.transform.localScale = initialScale;
        if (SecondClip != null)
        {
            robotAudio.clip = SecondClip;
            robotAudio.Play();
        }
    }
}
