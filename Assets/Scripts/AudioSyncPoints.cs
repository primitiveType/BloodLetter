using UnityEngine;

public class AudioSyncPoints : MonoBehaviour
{
    private AudioSource Source { get; set; }

    [SerializeField] private Metronome m_Metronome;
    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
        m_Metronome.OnBeat += MetronomeOnOnBeat;
    }

    private void MetronomeOnOnBeat()
    {
        Debug.Log("beat");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        m_Metronome.OnBeat -= MetronomeOnOnBeat;
    }
}
