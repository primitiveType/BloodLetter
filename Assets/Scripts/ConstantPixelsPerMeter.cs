using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantPixelsPerMeter : MonoBehaviour
{
    [SerializeField] private float m_TargetPixelsPerMeter = 60;
    private static readonly int NumSteps = Shader.PropertyToID("NumSteps");

    private Material Material { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Material = GetComponent<MeshRenderer>().material;
        UpdatePpm();
    }
    

    private void UpdatePpm()
    {
        Material.SetFloat(NumSteps, m_TargetPixelsPerMeter * transform.lossyScale.x);//assumes uniform scaling
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePpm();
    }
}
