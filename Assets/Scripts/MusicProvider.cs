﻿using System;
using CodingEssentials.Collections;
using E7.Introloop;
using UnityEngine;
using UnityEngine.Audio;

public class MusicProvider : MonoBehaviour
{
    public IntroloopAudio Music;
    public float Volume = 1f;
 
    private void Start()
    {
        IntroloopPlayer.Instance.Play(Music, 1);
        IntroloopPlayer.Instance.TemplateSource.outputAudioMixerGroup = SettingsManager.Instance.MusicMixerGroup;
        IntroloopPlayer.Instance.SetMixerGroup(SettingsManager.Instance.MusicMixerGroup);

    }

    private void Update()
    {//hack
        //IntroloopPlayer.Instance.InternalAudioSources.ForEach(ap => ap.volume = Volume);
       // IntroloopPlayer.Instance.InternalAudioSources.ForEach(ap => ap.outputAudioMixerGroup = SettingsManager.Instance.MusicMixerGroup);
    }
}