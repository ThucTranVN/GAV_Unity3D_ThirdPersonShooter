using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public Sound[] backgroundMusic;
    public Sound[] soundEffect;

    private int currentPlayingBGMIndex = 999;
    private bool shouldPlayBGM = false;
    private float bgmVolume;
    private float effectVolume;

    protected override void Awake()
    {
        base.Awake();

        bgmVolume = PlayerPrefs.GetFloat("BGM", 0.75f);
        effectVolume = PlayerPrefs.GetFloat("Effect", 0.75f);

        CreateAudioSource(backgroundMusic, bgmVolume);
        CreateAudioSource(soundEffect, effectVolume);
    }

    void Update() //for 1 bgm
    {
        if(currentPlayingBGMIndex !=999 && !backgroundMusic[currentPlayingBGMIndex].source.isPlaying)
        {
            currentPlayingBGMIndex++;
            if(currentPlayingBGMIndex >= backgroundMusic.Length)
            {
                currentPlayingBGMIndex = 0;
            }
            backgroundMusic[currentPlayingBGMIndex].source.Play();
        }
    }

    private void CreateAudioSource(Sound[] sounds, float volume)
    {
        foreach (Sound sound in sounds)//loop through each bgm/effect
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlayEffect(string name)
    {
        Sound effect = Array.Find(soundEffect, effect => effect.name == name);
        if(effect == null)
        {
            Debug.LogError("Unable to play effect " + name);
            return;
        }
        effect.source.Play();
    }

    public void PlayBackgroudMusic()
    {
        if(shouldPlayBGM == false)
        {
            shouldPlayBGM = true;
            currentPlayingBGMIndex = UnityEngine.Random.Range(0, backgroundMusic.Length - 1);
            backgroundMusic[currentPlayingBGMIndex].source.volume = backgroundMusic[currentPlayingBGMIndex].volume * bgmVolume;
            backgroundMusic[currentPlayingBGMIndex].source.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if(shouldPlayBGM == true)
        {
            shouldPlayBGM = false;
            currentPlayingBGMIndex = 999;
        }
    }

    public string GetBGMName()
    {
        return backgroundMusic[currentPlayingBGMIndex].name;
    }

    public void SetBGMVolume(float volume)
    {
        foreach (Sound bgm in backgroundMusic)
        {
            float curBGMVolume = PlayerPrefs.GetFloat("BGM", 0.75f);
            if(volume != curBGMVolume)
            {
                bgm.source.volume = volume;
                PlayerPrefs.SetFloat("BGM", volume);
            }
        }
    }

    public void SetEffectVolume(float volume)
    {
        float curEffectVolume = PlayerPrefs.GetFloat("Effect", 0.75f);
        foreach (Sound effect in soundEffect)
        {
            if(volume != curEffectVolume)
            {
                effect.source.volume = volume;
                PlayerPrefs.SetFloat("Effect", volume);
            }
        }
    }
}
