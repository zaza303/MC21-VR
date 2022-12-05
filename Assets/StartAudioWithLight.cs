using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class StartAudioWithLight : MonoBehaviour
{
    public GameObject SpotLight;
    public GameObject Sound;
    public GameObject NextDesk;

    private Light light;
    private AudioSource audio;
    private float audioLength;

    // Start is called before the first frame update
    void Start()
    {
        light = SpotLight.GetComponent<Light>();
        audio = Sound.GetComponent<AudioSource>();
        audioLength = audio.clip.length;

        light.gameObject.SetActive(false);
        NextDesk.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void BClick()
    {
        if (!audio.isPlaying) audio.Play();
        else audio.Stop();
        Invoke("TurnOnLight", audioLength);

    }

    void TurnOnLight()
    {
        light.gameObject.SetActive(true);
        NextDesk.gameObject.SetActive(true);
    }
}
