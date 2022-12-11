using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class StartAudioWithLight : MonoBehaviour
{
    public GameObject SpotLight;
    public GameObject Sound;
    public GameObject NextDesk;
    public GameObject Robot;

    private Light light;
    private AudioSource audio;
    private float audioLength;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        light = SpotLight.GetComponent<Light>();
        audio = Sound.GetComponent<AudioSource>();
        audioLength = audio.clip.length;
        anim = Robot.GetComponent<Animator>();

        light.gameObject.SetActive(false);
        NextDesk.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void BClick()
    {
<<<<<<< Updated upstream
        if (!audio.isPlaying) audio.Play();
        else audio.Stop();
        Invoke("TurnOnLight", audioLength);
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
=======
        if (!audio.isPlaying) {
            Invoke("TurnOnLight", audioLength);
            audio.Play(); 
        }
        else
        {
            audio.Stop();
            TurnOnLight();
        }
        

>>>>>>> Stashed changes
    }

    void TurnOnLight()
    {
        light.gameObject.SetActive(true);
        NextDesk.gameObject.SetActive(true);
    }
}
