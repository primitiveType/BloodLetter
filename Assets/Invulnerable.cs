using System;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    private float timestamp;
    [SerializeField] private float duration = 5;
    private IPostProcessHandle ppHandle;
    public void Awake()
    {
        timestamp = Time.time;
        ppHandle = PostProcessingManager.Instance.EnableInvulnEffect();
    }

    private void Update()
    {
        if (timestamp + duration < Time.time)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        ppHandle?.Dispose();
    }
}