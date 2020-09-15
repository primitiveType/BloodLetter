using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Profiling;

public class RandomDecalAtlas : MonoBehaviour
{
    [SerializeField] private UltimateDecal decal;
    [SerializeField] private int min;
    [SerializeField] private int max;

    [SerializeField] private float minAlpha;
    [SerializeField] private float maxAlpha;
    private static readonly int AlphaCutoff = Shader.PropertyToID("_MaskThreshold");
    [SerializeField] private bool interpolateAlpha;
    [SerializeField] private float secondsInterpolation = 5;
    public bool loops;

    void Awake()
    {
        decal.atlasIndex = Random.Range(min, max);
        if (interpolateAlpha)
        {
            StartCoroutine(ChangeAlphaAsync());
        }
        else
        {
            decal.material.SetFloat(AlphaCutoff, Random.Range(minAlpha, maxAlpha));
        }
    }

    private IEnumerator ChangeAlphaAsync()
    {
        float time = 0;
        while (time < secondsInterpolation || loops)
        {
            time += Time.deltaTime;
            var t = time / secondsInterpolation;

            //probably creating a new material over and over smh

            var value = Mathf.Lerp(maxAlpha, minAlpha, t);

            decal.alphaCutoff = value;
            Profiler.BeginSample("Update decal");
            UD_Manager.UpdateDecal(decal);//TODO: move this call into the property for alpha cutoff
            Profiler.EndSample();

            yield return new WaitForFixedUpdate();
            if (loops && time >= secondsInterpolation)
            {
                time = 0;
                yield return new WaitForSeconds(2f);
            }
        }

        decal.alphaCutoff = minAlpha;
    }
}