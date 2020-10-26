using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

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
    private const string AlphaSuffix = "_Alpha";
    private const string DiffuseSuffix = "_FrameBuffer";
    private const string NormalSuffix = "_Normal";

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


    private Task<Texture> GetTexture(string name)
    {
        if (Application.isPlaying)
        {
            return Addressables.LoadAssetAsync<Texture>(name).Task;
        }
#if UNITY_EDITOR
        else
        {
            return EditorLoad(name);
        }
#endif
        return null;
    }

#if UNITY_EDITOR

    private static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();
    private static Task<Texture> EditorLoad(string name)
    {
        if (Textures.ContainsKey(name))
        {
            return Task.FromResult(Textures[name]);
        }

        var assets = AssetDatabase.FindAssets(name);

        if (assets.Length == 0)
        {
            Debug.Log($"Unable to find texture for {name}");
            Textures[name] = null;
            return null;
        }


        var tex = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(assets[0]));
        Textures[name] = tex;
        return Task.FromResult(tex);
    }
#endif
    public Task<Texture> GetNormalMap() => GetTexture(AnimationName + NormalSuffix);


    public Task<Texture> GetDiffuseMap() =>
        GetTexture(AnimationName + DiffuseSuffix);

    public Task<Texture> GetAlphaMap() =>
        GetTexture(AnimationName + AlphaSuffix);


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
        var normalMapTask = GetNormalMap();
        var alphaMapTask = GetAlphaMap();
        var diffuseMapTask = GetDiffuseMap();


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