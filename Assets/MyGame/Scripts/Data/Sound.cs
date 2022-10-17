using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name; //Name of BGM/Effect
    public AudioClip clip; //Audio clip file
    [Range(0f,1f)]       //Limit volume range
    public float volume; //Store volume
    [Range(0.1f, 3f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source; //Source play the sound
    public bool loop = false; //Sound should loop?
}
