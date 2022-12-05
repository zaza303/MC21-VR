using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class StartAudioWithLight : MonoBehaviour
{
    public GameObject SpotLight;
    public GameObject Sound;
    private Light light;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        light = SpotLight.GetComponent<Light>();
        audio = Sound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void BClick()
    {
        if (!audio.isPlaying) audio.Play();
        else audio.Stop();


    }
    public void Update()
    {
        light.intensity = audio.isPlaying ? 0 : 2;
    }
}
