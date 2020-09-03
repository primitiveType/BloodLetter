using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddUvOffset : MonoBehaviour
{
    public Renderer toChange;
    public Vector2 OffsetToAdd;
    public float TimeStep;
    public string ShaderPropertyId = "UvOffset";

    private static readonly int Offset = Shader.PropertyToID("UvOffset");

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UvOffsetCr());
    }

    private IEnumerator UvOffsetCr()
    {
        Vector2 offset = OffsetToAdd;
        while (true)
        {
            yield return new WaitForSeconds(TimeStep);
            toChange.material.mainTextureOffset = offset;
            toChange.material.SetVector(Offset, offset);
            offset += OffsetToAdd;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}