using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class AnimationMaterialDictionary : ScriptableObject
{
    [SerializeField] private List<AnimationMaterialPropertyBlock> PropertyBlocksByModelAnimation;

    public static readonly int NumPixelsPerMeter = 20;
    #if UNITY_EDITOR
    public void AddPropertyBlock(Texture2DArray diffuse, Texture2DArray alpha, Texture2DArray normals, string modelName,
        string animationName, int columns, int rows, int numFrames)
    {
        if (PropertyBlocksByModelAnimation == null)
        {
            PropertyBlocksByModelAnimation = new List<AnimationMaterialPropertyBlock>();
        }

        string key = $"{modelName}_{animationName}";
        var block = new AnimationMaterialPropertyBlock
        {
            AnimationName = key,
            Columns = columns,
            Rows = rows,
            NumFrames = numFrames,
            DiffuseMap = diffuse,
            AlphaMap = alpha,
            NormalMap = normals
        };
        var oldItem = PropertyBlocksByModelAnimation.FirstOrDefault(item => item.AnimationName == key);
        if (oldItem != null)
        {
            PropertyBlocksByModelAnimation.Remove(oldItem);
        }

        PropertyBlocksByModelAnimation.Add(block);
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public void DebugMe()
    {
        foreach (var anim in PropertyBlocksByModelAnimation)
        {
            Debug.Log(anim.AnimationName);
        }
    }

    #endif
    

    public MaterialPropertyBlock GetPropertyBlock(MaterialPropertyBlock propertyBlock, string key)
    {
        var block = PropertyBlocksByModelAnimation.FirstOrDefault(item => item.AnimationName == key);
        if (block == null)
        {
            Debug.LogError($"Block is null for {key}");
            return null;
        }

        return block.GetMaterialPropertyBlock(propertyBlock);
    }
}