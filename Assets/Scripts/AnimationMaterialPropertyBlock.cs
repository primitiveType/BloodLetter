﻿using System;
using UnityEngine;

[Serializable]
public class AnimationMaterialPropertyBlock
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private string animationName;
    [SerializeField] private int numFrames;
    [SerializeField] private Texture2DArray normalMap;
    [SerializeField] private Texture2DArray diffuseMap;
    [SerializeField] private Texture2DArray alphaMap;
    public static readonly int RowsProperty = Shader.PropertyToID("Rows");
    public static readonly int FrameWidthProperty = Shader.PropertyToID("FrameWidth");
    public static readonly int FrameHeightProperty = Shader.PropertyToID("FrameHeight");
    public static readonly int ColumnsProperty = Shader.PropertyToID("Columns");
    public static readonly int NumFramesProperty = Shader.PropertyToID("NumFrames");
    public static readonly int TexturesProperty = Shader.PropertyToID("Textures");
    public static readonly int AlphaProperty = Shader.PropertyToID("Alpha");
    public static readonly int NormalsProperty = Shader.PropertyToID("Normals");

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

    public Texture2DArray NormalMap
    {
        get => normalMap;
        set => normalMap = value;
    }

    public Texture2DArray DiffuseMap
    {
        get => diffuseMap;
        set => diffuseMap = value;
    }

    public Texture2DArray AlphaMap
    {
        get => alphaMap;
        set => alphaMap = value;
    }

    public MaterialPropertyBlock GetMaterialPropertyBlock()
    {
        var block = new MaterialPropertyBlock();
        block.SetInt(RowsProperty, Rows);
        block.SetInt(ColumnsProperty, Columns);
        block.SetInt(NumFramesProperty, NumFrames);
        block.SetTexture(NormalsProperty, NormalMap);
        block.SetTexture(AlphaProperty, AlphaMap);
        block.SetTexture(TexturesProperty, DiffuseMap);
        block.SetInt(FrameWidthProperty, diffuseMap.width / columns);
        block.SetInt(FrameHeightProperty, diffuseMap.height / rows);
        return block;
    }
}