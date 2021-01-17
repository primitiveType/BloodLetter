using System.Collections;
using UnityEngine;

public class AddUvOffset : MonoBehaviour
{
    private static readonly int Offset = Shader.PropertyToID("UvOffset");
    public Vector2 OffsetToAdd;
    public string ShaderPropertyId = "UvOffset";
    public float TimeStep;
    public Renderer toChange;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(UvOffsetCr());
    }

    private IEnumerator UvOffsetCr()
    {
        var offset = OffsetToAdd;
        while (true)
        {
            yield return new WaitForSeconds(TimeStep);
            toChange.material.mainTextureOffset = offset;
            toChange.material.SetVector(Offset, offset);
            offset += OffsetToAdd;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}