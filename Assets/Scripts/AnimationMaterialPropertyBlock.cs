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

    private static Dictionary<string, Task<Texture>> TextureTasks = new Dictionary<string, Task<Texture>>();

    private AsyncOperationHandle<Texture> GetTexture(string name)
    {
        if (Application.isPlaying)
        {
            // if (TextureTasks.TryGetValue(name, out Task<Texture> task))
            // {
            //     return task;
            // }
            // else
            // {
            return Addressables.LoadAssetAsync<Texture>(name);
            // }
        }
#if UNITY_EDITOR
        else
        {
            var handle = new AsyncOperationHandle<UnityEngine.Texture>();
            return handle;
            // return EditorLoad(name);
        }
#endif
        return new AsyncOperationHandle<UnityEngine.Texture>();
    }

#if UNITY_EDITOR

    private static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

    private static Texture EditorLoad(string name)
    {
        if (Textures.ContainsKey(name))
        {
            return Textures[name];
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
        return tex;
    }

    public Texture EditorGetNormalMap() => EditorLoad(AnimationName + NormalSuffix);


    public Texture EditorGetDiffuseMap() =>
        EditorLoad(AnimationName + DiffuseSuffix);

    public Texture EditorGetAlphaMap() =>
        EditorLoad(AnimationName + AlphaSuffix);

#endif
    public AsyncOperationHandle<Texture> GetNormalMap() => GetTexture(AnimationName + NormalSuffix);


    public AsyncOperationHandle<Texture> GetDiffuseMap() =>
        GetTexture(AnimationName + DiffuseSuffix);

    public AsyncOperationHandle<Texture> GetAlphaMap() =>
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
        Texture normalMap;
        Texture diffuseMap;
        Texture alphaMap;
#if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            normalMap = EditorGetNormalMap();
            diffuseMap = EditorGetDiffuseMap();
            alphaMap = EditorGetAlphaMap();
        }
        else
        {
#endif

            var normalMapTask = GetNormalMap();
            var alphaMapTask = GetAlphaMap();
            var diffuseMapTask = GetDiffuseMap();


            block.SetInt(RowsProperty, Rows);
            block.SetInt(ColumnsProperty, Columns);
            block.SetInt(NumFramesProperty, NumFrames);

            block.SetFloat(GroundPositionProperty, NormalizedGroundPosition);

            while (!(alphaMapTask.IsDone && normalMapTask.IsDone && diffuseMapTask.IsDone))
            {
                await Task.Delay(1);
            }

            normalMap = normalMapTask.Result;
            diffuseMap = diffuseMapTask.Result;
            alphaMap = alphaMapTask.Result;
#if UNITY_EDITOR
        }
#endif

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