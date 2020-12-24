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

    void Start()
    {
        PropertyHash = Shader.PropertyToID(m_PropertyName);
        Material = GetComponent<Renderer>().material;
        Material.SetFloat(PropertyHash, m_Min);
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

        Material.SetFloat(PropertyHash, value);
    }
}