using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviourSingleton<MusicManager>
{
    [SerializeField] private AudioSource m_IntroAudioSource;
    [SerializeField] private AudioSource m_MainAudioSource;

    [SerializeField] private AudioClip introClip; 
    [SerializeField] private AudioClip mainClip; 
    // Start is called before the first frame update
    void Start()
    {
        m_IntroAudioSource.clip = introClip;
        m_MainAudioSource.clip = mainClip;
        m_IntroAudioSource.Play();
        m_MainAudioSource.PlayScheduled(AudioSettings.dspTime + introClip.length);
    }
}
