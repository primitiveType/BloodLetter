using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ScreenWipeManager : MonoBehaviourSingleton<ScreenWipeManager>
{
    private RenderTexture captureTexture;

    [SerializeField] private Material wipeMaterial;
    [SerializeField] private float wipeDuration;
    [SerializeField] private RawImage Image;
    private static readonly int WipeAmount = Shader.PropertyToID("_WipeAmount");
    private static readonly int Seed = Shader.PropertyToID("Seed");    


    private Coroutine wipeRoutine;
    private Random Random { get; set; }

    private void Start()
    {
        wipeMaterial.SetFloat(WipeAmount, 1);
        Random = new Random();
    }

    public void DoWipe()
    {
        if (wipeRoutine != null)
        {
            StopCoroutine(wipeRoutine);
        }

        wipeRoutine = StartCoroutine(WipeCr());
    }

    public void Capture(Camera toCapture, bool applyImage)
    {
        if (toCapture == null)
            return;
        
        if (captureTexture == null || captureTexture.height != Screen.height || captureTexture.width != Screen.width)
        {
            Debug.Log("creating screen texture for wipe.");
            captureTexture = new RenderTexture(Screen.width, Screen.height, 24);
            captureTexture.Create();
        }

        // captureTexture.height = Screen.height;
        // captureTexture.width = Screen.width;
        var prevTarget = toCapture.targetTexture;
        toCapture.targetTexture = captureTexture;
        toCapture.Render();
        toCapture.targetTexture = prevTarget;
        wipeMaterial.mainTexture = captureTexture;
        Image.texture = captureTexture;
        if (applyImage)
        {
            wipeMaterial.SetFloat(WipeAmount, 0);
        }
    }

    private IEnumerator WipeCr()
    {
        wipeMaterial.SetFloat(Seed, Random.Next(0, 100));
        float t = 0;
        while (t < wipeDuration)
        {
            wipeMaterial.SetFloat(WipeAmount, t / wipeDuration);
            yield return null;
            t += Time.unscaledDeltaTime;
        }

        wipeMaterial.SetFloat(WipeAmount, 1f);
    }
}