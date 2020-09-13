using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

public class RandomDecalAtlas : MonoBehaviour
{
    [SerializeField]private UltimateDecal decal;
    [SerializeField]private int min;
    [SerializeField]private int max;
    
    [SerializeField]private float minAlpha;
    [SerializeField]private float maxAlpha;
    private static readonly int AlphaCutoff = Shader.PropertyToID("_Cutoff");

    void Awake()
    {
        decal.atlasIndex = Random.Range(min, max);
        decal.material.SetFloat(AlphaCutoff, Random.Range(minAlpha, maxAlpha));
    }

    void Update()
    {
        
    }
}
