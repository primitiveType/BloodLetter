using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class AnimationMaterialDictionary : ScriptableObject
{
    public static readonly int NumPixelsPerMeter = 40;
    [SerializeField] private List<AnimationMaterialPropertyBlock> PropertyBlocksByModelAnimation;

    public List<AnimationMaterialPropertyBlock> PropertyBlocks => PropertyBlocksByModelAnimation.ToList();

    public Task<MaterialPropertyBlock> GetPropertyBlock(MaterialPropertyBlock propertyBlock, string key)
    {
        var block = PropertyBlocksByModelAnimation.FirstOrDefault(item => item.AnimationName == key);
        if (block == null)
        {
            Debug.LogError($"Block is null for {key}");
            return null;
        }

        return block.GetMaterialPropertyBlock(propertyBlock);
    }
#if UNITY_EDITOR
    // public void AddPropertyBlock(Texture2DArray diffuse, Texture2DArray alpha, Texture2DArray normals, string modelName,
    //     string animationName, float groundPos, int columns, int rows, int numFrames)
    // {
    //     if (PropertyBlocksByModelAnimation == null)
    //         PropertyBlocksByModelAnimation = new List<AnimationMaterialPropertyBlock>();
    //
    //     var key = $"{modelName}_{animationName}";
    //     var block = new AnimationMaterialPropertyBlock
    //     {
    //         AnimationName = key,
    //         Columns = columns,
    //         Rows = rows,
    //         NumFrames = numFrames,
    //         NormalizedGroundPosition = groundPos,
    //         DiffuseMap = diffuse,
    //         AlphaMap = alpha,
    //         NormalMap = normals
    //     };
    //     var oldItem = PropertyBlocksByModelAnimation.FirstOrDefault(item => item.AnimationName == key);
    //     if (oldItem != null) PropertyBlocksByModelAnimation.Remove(oldItem);
    //
    //     PropertyBlocksByModelAnimation.Add(block);
    //     EditorUtility.SetDirty(this);
    //     AssetDatabase.SaveAssets();
    // }

    public void DebugMe()
    {
        foreach (var anim in PropertyBlocksByModelAnimation) Debug.Log(anim.AnimationName);
    }

#endif
    public Texture2DArray GetAlpha(string key)
    {
        var block = PropertyBlocksByModelAnimation.FirstOrDefault(item => item.AnimationName == key);
        if (block == null)
        {
            Debug.LogError($"Block is null for {key}");
            return null;
        }

        return block.GetAlphaMap().Result as Texture2DArray;
    }
}