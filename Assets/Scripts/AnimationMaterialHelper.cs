using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

    [SerializeField] private Transform anchorTransform;
    [SerializeField] private Collider anchorTransformCollider;

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();


    private void Awake()
    {
        // MyRenderer.material = new Material(MyRenderer.sharedMaterial);
        if (anchorTransform == null)
        {
            anchorTransform = materialGameObject.transform;
        }


    }


    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (EditorApplication.isPaused)
        {
            return;
        }
        #endif
        
        if (Application.isEditor && !Application.isPlaying )
        {
            //no null ref, no null ref, stop!
            CurrentAnimation = "";
            var animator = materialGameObject.GetComponent<Animator>();
            if (animator != null && animator.isInitialized)
            {
                var modelName = animator.runtimeAnimatorController.name.Replace("_Animator_Controller", "");
                modelName = modelName.Replace("_Animator", "");
                var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                AnimationStarted($"{modelName}_{animationName}");
            }
        }
    }

    private string CurrentAnimation { get; set; }

    public bool resize = true;
    public bool reposition = true;

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


            if (resize)
            {
                var pxPerMeter = AnimationMaterialDictionary.NumPixelsPerMeter;

                var width = (float) block.GetInt(AnimationMaterialPropertyBlock.FrameWidthProperty) /
                            pxPerMeter;
                var height = (float) block.GetInt(AnimationMaterialPropertyBlock.FrameHeightProperty) /
                             pxPerMeter;
                anchorTransform.localScale =
                    new Vector3((float) width, (float) height, 1);
            }

            if (reposition)
            {
                float yFudge = .01f;
                if (anchorTransformCollider)
                {
                    yFudge = anchorTransformCollider.bounds.size.y / 2f;
                }

                anchorTransform.localPosition = new Vector3(0, (anchorTransform.localScale.y / 2) - yFudge, 0);
            }
        }
    }

    private MaterialPropertyBlock cachedPropertyBlock;
    private string AnimationUsedForLastAlphaCheck;
    private Texture2DArray cachedAlpha;
    private Color[] cachedAlphaPixels;

    /// <summary>
    /// returns true if the coordinate is alpha > .5f for the current anim frame
    /// </summary>
    /// <returns></returns>
    public bool QueryAlpha(Vector2 textureCoord)
    {
        int perspective;
        // if (AnimationUsedForLastAlphaCheck != CurrentAnimation)
        {
            cachedPropertyBlock = new MaterialPropertyBlock();
            MyRenderer.GetPropertyBlock(cachedPropertyBlock);
            Debug.Log("getting texture array for alpha check");
            AnimationUsedForLastAlphaCheck = CurrentAnimation;
            cachedAlpha = (Texture2DArray) (cachedPropertyBlock).GetTexture(Alpha);

            perspective =
                Mathf.Clamp(cachedPropertyBlock.GetInt(Perspective), 0,
                    7); //it's actually possible to get back 8 from this which is invalid.
            cachedAlphaPixels = cachedAlpha.GetPixels(perspective);
        }
        // else
        // {
        //     Debug.Log("Reusing texture array");
        //     perspective =
        //         Mathf.Clamp(cachedPropertyBlock.GetInt(Perspective), 0,
        //             7); //it's actually possible to get back 8 from this which is invalid.
        // }


        int totalWidth = cachedAlpha.width;
        int totalHeight = cachedAlpha.height;
        int frame = cachedPropertyBlock.GetInt(Frame);


        int rows = cachedPropertyBlock.GetInt(Rows);
        int columns = cachedPropertyBlock.GetInt(Columns);
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
        int index = (yPos * cachedAlpha.width) + xPos;
        //.GetPixel( , );
        if (index > cachedAlphaPixels.Count())
        {
            Debug.LogError("index out of range!");
        }

        Color color = cachedAlphaPixels[index];
        return color.r > .5f;
    }
}