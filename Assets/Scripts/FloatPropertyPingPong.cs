using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPropertyPingPong : MonoBehaviour
{
    [SerializeField] private string m_PropertyName;
    private int PropertyHash { get; set; }
    private Material Material { get; set; }
    [SerializeField] private float m_Min;
    [SerializeField] private float m_Max;
    [SerializeField] private float m_Speed;

    [SerializeField] private EasingFunction.Ease EasingMethod;

    private bool reverse;
    private float time;
    private Renderer m_Renderer;

    void Start()
    {
        PropertyHash = Shader.PropertyToID(m_PropertyName);
        m_Renderer = GetComponent<Renderer>();
        Material = m_Renderer.material;
        SetValue(m_Min);
    }

    // Update is called once per frame
    void Update()
    {
        time += m_Speed * Time.deltaTime;
        if (time >= 1)
        {
            reverse = !reverse;
            time = 0;
        }

        float value;
        if (reverse)
        {
            value = EasingFunction.GetEasingFunction(EasingMethod)(m_Max, m_Min, time);
        }
        else
        {
            value = EasingFunction.GetEasingFunction(EasingMethod)(m_Min, m_Max, time);
        }

        SetValue(value);
    }

    private void SetValue(float value)
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        m_Renderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetFloat(PropertyHash, value);
        m_Renderer.SetPropertyBlock(materialPropertyBlock);
    }
}