using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class AnimationMaterialHelper : MonoBehaviour
{
    [SerializeField] private AnimationMaterialDictionary _dictionary;

    // [SerializeField] private string ModelName;
    [SerializeField] private GameObject materialGameObject;

    private Renderer m_MyRenderer;
    private static readonly int Alpha = Shader.PropertyToID("Alpha");
    private static readonly int Perspective = Shader.PropertyToID("Perspective");
    private static readonly int Columns = Shader.PropertyToID("Columns");
    private static readonly int Rows = Shader.PropertyToID("Rows");
    private static readonly int Frame = Shader.PropertyToID("Frame");

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();


    private void Awake()
    {
        MyRenderer.material = new Material(MyRenderer.sharedMaterial);
    }


    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            //no null ref, no null ref, stop!
            CurrentAnimation = "";
            var animator = materialGameObject.GetComponent<Animator>();
            if (animator != null)
            {
                var modelName = animator.runtimeAnimatorController.name.Replace("_Animator_Controller", "");
                var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                AnimationStarted($"{modelName}_{animationName}");
            }
        }
    }

    private string CurrentAnimation { get; set; }

    public void AnimationStarted(string animationName)
    {
        if (animationName != CurrentAnimation)
        {
            CurrentAnimation = animationName;
            // Debug.Log($"{animationName} started!");
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            MyRenderer.GetPropertyBlock(block);
            _dictionary.GetPropertyBlock(block, animationName);

            MyRenderer.SetPropertyBlock(block);
            var pxPerMeter = AnimationMaterialDictionary.NumPixelsPerMeter;

            var meshMultiplier = 1f;

            var width = (float) meshMultiplier * block.GetInt(AnimationMaterialPropertyBlock.FrameWidthProperty) /
                        pxPerMeter;
            var height = (float) meshMultiplier * block.GetInt(AnimationMaterialPropertyBlock.FrameHeightProperty) /
                         pxPerMeter;
            materialGameObject.transform.localScale =
                new Vector3((float) width, (float) height, 1);
            float yFudge = .25f;
            materialGameObject.transform.localPosition = new Vector3(0, (height / 2) - yFudge, 0);
        }
    }


    /// <summary>
    /// returns true if the coordinate is alpha > .5f for the current anim frame
    /// </summary>
    /// <returns></returns>
    public bool QueryAlpha(Vector2 textureCoord)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        MyRenderer.GetPropertyBlock(block);


        Texture2DArray tex = (Texture2DArray) (block).GetTexture(Alpha);
        int totalWidth = tex.width;
        int totalHeight = tex.height;
        int perspective =
            Mathf.Clamp(block.GetInt(Perspective), 0,
                7); //it's actually possible to get back 8 from this which is invalid.
        int frame = block.GetInt(Frame);


        int rows = block.GetInt(Rows);
        int columns = block.GetInt(Columns);
        float frameWidth = (float) totalWidth / columns;
        float frameHeight = (float) totalHeight / rows;
        int numFrames = rows * columns; //includes "blank" frames purposefully

        frame = frame % numFrames; //TODO: consider removing this after resolving bug in exporter

        int columnOfFrame = (frame % columns);
        float minXPixel = (columnOfFrame / (float) columns) * totalWidth;
        float maxXPixel = minXPixel + frameWidth;

        int rowOfFrame = rows - 1 - (frame / columns); //frames count from top left to bottom right unlike pixel coords
        float minYPixel = ((rowOfFrame) / (float) rows) * totalHeight;
        float maxYPixel = minYPixel + frameHeight;

        Vector2 pixelCoord = new Vector2((frameWidth * textureCoord.x) + minXPixel,
            (frameHeight * textureCoord.y) + minYPixel);

        int xPos = Mathf.FloorToInt(pixelCoord.x);
        int yPos = Mathf.FloorToInt(pixelCoord.y);
        int index = (yPos * tex.width) + xPos;
        //.GetPixel( , );
        Color[] colors = tex.GetPixels(perspective);
        if (index > colors.Count())
        {
            Debug.LogError("index out of range!");
        }

        Color color = colors[index];
        var numTransparent = colors.Count(c => c.r < .5f);
        // Debug.Log(
        //     $"{colors[0].ToString()} vs {colors[colors.Length - 2].ToString()}.Hit color {color.ToString()} on frame {frame} at coords {xPos}x{yPos}. {numTransparent}/{colors.Count()} pixels were alpha.");
        return color.r > .5f;
    }
}