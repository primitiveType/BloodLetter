using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantPixelsPerMeter : MonoBehaviour
{
    [SerializeField] private float m_TargetPixelsPerMeter = 60;
    private static readonly int NumSteps = Shader.PropertyToID("NumSteps");
    public MeshRenderer Mesh { get; private set; }

    private Material Material { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
        Material = Mesh.material;
        UpdatePpm();
    }
    

    private void UpdatePpm()
    {
        var block = new MaterialPropertyBlock();
        Mesh.GetPropertyBlock(block);
        block.SetFloat(NumSteps, m_TargetPixelsPerMeter * transform.lossyScale.x);
        Mesh.SetPropertyBlock(block);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePpm();
    }
}
