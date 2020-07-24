using System;
using UnityEngine;

public class Timer : MonoBehaviourSingleton<Timer>
{

    private float seconds;
    private bool isPaused;

    private void Update()
    {
        if (!isPaused)
        {
            seconds += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        seconds = 0;
        isPaused = false;
    }

    public TimeSpan GetTime()
    {
        return new TimeSpan(0,0, (int)seconds);
    }

    public void PauseTimer()
    {
        isPaused = true;
    }
    
    public void UnPauseTimer()
    {
        isPaused = false;
    }

    public void ResetTimer()
    {
        seconds = 0;
    }
}