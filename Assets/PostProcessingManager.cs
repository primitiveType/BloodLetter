using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviourSingleton<PostProcessingManager>
{
    [SerializeField] private Volume Invuln;
    [SerializeField] private Volume GasMask;
    [SerializeField] private Volume Damage;
    [SerializeField] private Volume Death;

    Dictionary<Volume, int> HandlesPerEffect = new Dictionary<Volume, int>();

    protected override void Awake()
    {
        HandlesPerEffect.Add(Invuln, 0);
        HandlesPerEffect.Add(Damage, 0);
        HandlesPerEffect.Add(Death, 0);
    }

    public IPostProcessHandle EnableInvulnEffect()
    {
        return EnableEffect(Invuln);
    }
    public IPostProcessHandle EnableGasMaskEffect()
    {
        return EnableEffect(GasMask);
    }

    
    public IPostProcessHandle EnableDamagedEffect(float weight)
    {
        return EnableEffect(Damage, weight);
    }
    public IPostProcessHandle EnableDeathEffect()
    {
        return EnableEffect(Death);
    }

    private IPostProcessHandle EnableEffect(Volume effectGo, float weight = 1f)
    {
        HandlesPerEffect[effectGo]++;
        effectGo.gameObject.SetActive(true);
        effectGo.weight = weight;
        return new GameObjectActiveHandle(effectGo);
    }

    public void DisposeEffectHandle(Volume go)
    {
        HandlesPerEffect[go] -= 1;
        if (HandlesPerEffect[go] <= 0)
        {
            go.gameObject.SetActive(false);
        }
    }

    private class GameObjectActiveHandle : IPostProcessHandle
    {
        private Volume GameObject;

        public GameObjectActiveHandle(Volume go)
        {
            GameObject = go;
        }

        public void Dispose()
        {
            PostProcessingManager.Instance.DisposeEffectHandle(GameObject);
        }

        public void SetWeight(float weight)
        {
            PostProcessingManager.Instance.SetWeight(GameObject, weight);
        }
    }

    private void SetWeight(Volume pp, float weight)
    {
        pp.weight = weight;
    }
}

public interface IPostProcessHandle : IDisposable
{
    void SetWeight(float weight);
}