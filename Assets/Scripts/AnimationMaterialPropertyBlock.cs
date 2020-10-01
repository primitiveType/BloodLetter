using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class AnimationMaterialPropertyBlock
{
    public static readonly int RowsProperty = Shader.PropertyToID("Rows");
    public static readonly int FrameWidthProperty = Shader.PropertyToID("FrameWidth");
    public static readonly int FrameHeightProperty = Shader.PropertyToID("FrameHeight");
    public static readonly int ColumnsProperty = Shader.PropertyToID("Columns");
    public static readonly int NumFramesProperty = Shader.PropertyToID("NumFrames");
    public static readonly int GroundPositionProperty = Shader.PropertyToID("GroundPosition");
    public static readonly int TexturesProperty = Shader.PropertyToID("Textures");
    public static readonly int AlphaProperty = Shader.PropertyToID("Alpha");
    public static readonly int NormalsProperty = Shader.PropertyToID("Normals");
    private const string AlphaSuffix = "_Alpha_Array";
    private const string DiffuseSuffix = "_FrameBuffer_Array";

    private const string NormalSuffix = "_Normal_Array";

    // [SerializeField] private Texture alphaMap;
    [SerializeField] private string animationName;

    [SerializeField] private int columns;

    // [SerializeField] private Texture diffuseMap;
    [SerializeField] private float normalizedGroundPosition;

    // [SerializeField] private Texture normalMap;
    [SerializeField] private int numFrames;
    [SerializeField] private int rows;

    public int Rows
    {
        get => rows;
        set => rows = value;
    }

    public int Columns
    {
        get => columns;
        set => columns = value;
    }

    public string AnimationName
    {
        get => animationName;
        set => animationName = value;
    }

    public int NumFrames
    {
        get => numFrames;
        set => numFrames = value;
    }

    public float NormalizedGroundPosition
    {
        get => normalizedGroundPosition;
        set => normalizedGroundPosition = value;
    }

    private const string prefix = "Assets/SpriteOutputs/Enemies/HARPY_breastsExposed_LEGACY/";

    public AsyncOperationHandle<Texture> GetNormalMap() =>
        Addressables.LoadAssetAsync<Texture>( AnimationName + NormalSuffix);


    public AsyncOperationHandle<Texture> GetDiffuseMap() =>
        Addressables.LoadAssetAsync<Texture>( AnimationName + DiffuseSuffix);

    public AsyncOperationHandle<Texture> GetAlphaMap() =>
        Addressables.LoadAssetAsync<Texture>( AnimationName + AlphaSuffix);


    // public MaterialPropertyBlock GetMaterialPropertyBlock(MaterialPropertyBlock block)
    // {
    //     block.SetInt(RowsProperty, Rows);
    //     block.SetInt(ColumnsProperty, Columns);
    //     block.SetInt(NumFramesProperty, NumFrames);
    //     if (NormalMap != null) block.SetTexture(NormalsProperty, NormalMap);
    //
    //     block.SetFloat(GroundPositionProperty, NormalizedGroundPosition);
    //     block.SetTexture(AlphaProperty, AlphaMap);
    //     block.SetTexture(TexturesProperty, DiffuseMap);
    //     block.SetInt(FrameWidthProperty, diffuseMap.width / columns);
    //     block.SetInt(FrameHeightProperty, diffuseMap.height / rows);
    //     return block;
    // }
    public async Task<MaterialPropertyBlock> GetMaterialPropertyBlock(MaterialPropertyBlock block)
    {
        var normalMapTask = GetNormalMap().Task;
        var alphaMapTask = GetAlphaMap().Task;
        var diffuseMapTask = GetDiffuseMap().Task;


        block.SetInt(RowsProperty, Rows);
        block.SetInt(ColumnsProperty, Columns);
        block.SetInt(NumFramesProperty, NumFrames);

        block.SetFloat(GroundPositionProperty, NormalizedGroundPosition);

        await Task.WhenAll(new List<Task>
        {
            normalMapTask, alphaMapTask, diffuseMapTask
        });


        var normalMap = normalMapTask.Result;
        var diffuseMap = diffuseMapTask.Result;
        var alphaMap = alphaMapTask.Result;
        if (normalMap != null) block.SetTexture(NormalsProperty, normalMap);
        if (alphaMap != null) block.SetTexture(AlphaProperty, alphaMap);
        if (diffuseMap != null) block.SetTexture(TexturesProperty, diffuseMap);
        else
        {
            Debug.Log($"diffuse map null for {animationName}");
        }
        block.SetInt(FrameWidthProperty, diffuseMap.width / columns);
        block.SetInt(FrameHeightProperty, diffuseMap.height / rows);

        return block;
    }
}