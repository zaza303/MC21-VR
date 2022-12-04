using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class LightController : MonoBehaviour
{
    public GameObject spotLight;
    public GameObject sound;
    private Light light;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        light = spotLight.GetComponent<Light>();
        audio = sound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void BClick()
    {
        if (!audio.isPlaying) audio.Play();
        else audio.Stop();
        
        
    }
    public void Update()
    {
       light.intensity = audio.isPlaying ? 0:2;
    }
}
