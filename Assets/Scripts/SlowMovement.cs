using System;
using UnityEngine;

public class SlowMovement : MonoBehaviour
{
    [SerializeField] private float m_duration;
    private IPostProcessHandle m_EffectHandle;

    private MovementModifierHandle Handle { get; } = new MovementModifierHandle();

    private void Awake()
    {
        GetComponent<IMovementHandler>().AddMovementModifier(Handle);
    }

    //halve's movement speed
    private float Duration
    {
        get => m_duration;
        set
        {
            m_duration = Mathf.Clamp(value, 0, Single.PositiveInfinity);
            if (Math.Abs(Duration) < float.Epsilon)
            {
                Handle.ModifierPercentage = 0;
                // should check if player. also, this assumes slowing only happens from webs. 
                m_EffectHandle?.Dispose();
                m_EffectHandle = null;
            }
            else
            {
                Handle.ModifierPercentage = -.75f;

                if (m_EffectHandle == null)
                {
                    m_EffectHandle = PostProcessingManager.Instance.EnableWebEffect();
                }
            }
        }
    }

    public void AddDuration(float toAdd)
    {
        Duration += toAdd;
    }

    private void Update()
    {
        Duration -= Time.deltaTime;
    }
}