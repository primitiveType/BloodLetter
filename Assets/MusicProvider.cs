using E7.Introloop;
using UnityEngine;

public class MusicProvider : MonoBehaviour
{
    public IntroloopAudio Music;

    private void Start()
    {
        IntroloopPlayer.Instance.Play(Music);
    }
}