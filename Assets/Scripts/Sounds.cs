using UnityEngine;

public class Sounds : MonoBehaviourSingleton<Sounds>
{
    private AudioSource source;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}