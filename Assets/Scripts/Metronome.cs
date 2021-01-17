using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    //Timer Events based on the beat
    public delegate void Beat();

    public event Beat OnBeat;

    public delegate void DownBeat();

    public event DownBeat OnDownBeat;

    private double DownBeatTime { get; set; } = 0;
    private double LastDownBeatTime { get; set; } = 0;

    private double BeatTime { get; set; } = 0;
    private double LastBeatTime { get; set; } = 0;

    [SerializeField] private double m_Bpm = 140.0F;
    private float Gain { get; } = 0.5F;
    private int SignatureHi { get; } = 4;
    private int SignatureLo { get; } = 4;
    private bool PlayMetronomeTick { get; } = true;

    private double NextTick { get; set; } = 0.0F;
    private float Amp { get; set; } = 0.0F;
    private float Phase { get; set; } = 0.0F;
    private double SampleRate { get;  set; } = 0.0F;
    private int Accent { get;  set; }
    private bool Running { get;  set; } = false;

    public double Bpm
    {
        get => m_Bpm;
        set => m_Bpm = value;
    }


    void Start()
    {
        Accent = SignatureHi;
        double startTick = AudioSettings.dspTime;
        SampleRate = AudioSettings.outputSampleRate;
        NextTick = startTick * SampleRate;
        Running = true;
    }

    private void Update()
    {
        if (LastBeatTime == BeatTime)
        {
            if (LastDownBeatTime == DownBeatTime)
            {
                if (OnDownBeat != null)
                    OnDownBeat();
            }
            else
            {
                if (OnBeat != null)
                    OnBeat();
            }
        }

        DownBeatTime = AudioSettings.dspTime;
        BeatTime = AudioSettings.dspTime;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!Running)
            return;

        double samplesPerTick = SampleRate * 60.0F / Bpm * 4.0F / SignatureLo;
        double sample = AudioSettings.dspTime * SampleRate;

        int dataLen = data.Length / channels;
        int n = 0;

        while (n < dataLen)
        {
            float x = Gain * Amp * Mathf.Sin(Phase);
            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] += x;
                i++;
            }

            while (sample + n >= NextTick)
            {
                NextTick += samplesPerTick;
                if (PlayMetronomeTick)
                    Amp = 1.0F;
                if (++Accent > SignatureHi)
                {
                    Accent = 1;
                    if (PlayMetronomeTick)
                        Amp *= 2.0F;
                    LastDownBeatTime = AudioSettings.dspTime;
                }

                LastBeatTime = AudioSettings.dspTime;

                // Debug.Log("Tick: " + accent + "/" + signatureHi);
            }

            if (PlayMetronomeTick)
            {
                Phase += Amp * 0.3F;
                Amp *= 0.993F;
            }

            n++;
        }
    }
}