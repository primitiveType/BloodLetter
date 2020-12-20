using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[ExecuteAlways]
public class AnimationMaterialHelper : MonoBehaviour
{
    private static readonly int Alpha = Shader.PropertyToID("Alpha");
    private static readonly int Perspective = Shader.PropertyToID("Perspective");
    private static readonly int Columns = Shader.PropertyToID("Columns");
    private static readonly int Rows = Shader.PropertyToID("Rows");

    private static readonly int Frame = Shader.PropertyToID("Frame");
    // [SerializeField] private AssetReference _dictionaryReference;

    [SerializeField] private AnimationMaterialDictionary _dictionary;


    [SerializeField] private Transform anchorTransform;
    [SerializeField] private Collider anchorTransformCollider;
    private string AnimationUsedForLastAlphaCheck;
    private Texture2DArray cachedAlpha;
    private Color[] cachedAlphaPixels;

    private MaterialPropertyBlock cachedPropertyBlock;

    private Renderer m_MyRenderer;

    // [SerializeField] private string ModelName;
    [SerializeField] private GameObject materialGameObject;
    public bool reposition = true;

    public bool resize = true;


    [SerializeField] private float yFudge = .01f;
    [SerializeField] private string _currentAnimation;

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();

    private string CurrentAnimation
    {
        get => _currentAnimation;
        set => _currentAnimation = value;
    }


    private void Awake()
    {
        // _dictionaryReference.LoadAssetAsync<AnimationMaterialDictionary>().Completed +=
        //     handle => _dictionary = handle.Result;
        // MyRenderer.material = new Material(MyRenderer.sharedMaterial);
        if (anchorTransform == null) anchorTransform = materialGameObject.transform;

        if (Application.isPlaying)
            CurrentAnimation = "";

#if UNITY_EDITOR

#else
#endif
    }


    private void OnDestroy()
    {
        // _dictionaryReference.ReleaseAsset();
    }


    // Update is called once per frame
    private void Update()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPaused) return;
#endif

        if (Application.isEditor && !Application.isPlaying)
        {
            //no null ref, no null ref, stop!
            var animator = materialGameObject.GetComponent<Animator>();
            if (animator != null && animator.isInitialized)
            {
                var modelName = animator.runtimeAnimatorController.name.Replace("_Animator_Controller", "");
                modelName = modelName.Replace("_variant", "");
                modelName = modelName.Replace("_Animator", "");
                var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                AnimationStarted($"{modelName}_{animationName}");
            }
        }
    }

    public async void AnimationStarted(string animationName)
    {
        if (animationName != CurrentAnimation)
        {
            CurrentAnimation = animationName;
            var block = new MaterialPropertyBlock();
            await SetPropertyBlockAsync();


            async Task SetPropertyBlockAsync()
            {

                if (animationName != CurrentAnimation)
                {//A new task was started, we can effectively cancel this one.
                    return;
                }
                MyRenderer.GetPropertyBlock(block);
                block = await _dictionary.GetPropertyBlock(block, animationName);
                if (animationName != CurrentAnimation)
                {//A new task was started, we can effectively cancel this one.
                    return;
                }

                MyRenderer.SetPropertyBlock(block);
                var pxPerMeter = AnimationMaterialDictionary.NumPixelsPerMeter;

                var width = (float) block.GetInt(AnimationMaterialPropertyBlock.FrameWidthProperty) /
                            pxPerMeter;
                var height = (float) block.GetInt(AnimationMaterialPropertyBlock.FrameHeightProperty) /
                             pxPerMeter;
                var offset = (height / 2f) +
                             height * block.GetFloat(AnimationMaterialPropertyBlock.GroundPositionProperty);
                if (resize)
                    anchorTransform.localScale =
                        new Vector3(width, height, 1);

                if (reposition)
                {
                    anchorTransform.localPosition = new Vector3(0, offset, 0);
                }
            }
        }
    }


    /// <summary>
    ///     returns true if the coordinate is alpha > .5f for the current anim frame
    /// </summary>
    /// <returns></returns>
    public bool QueryAlpha(Vector2 textureCoord)
    {
        textureCoord = new Vector2(1 - textureCoord.x, textureCoord.y);//HACK texture x is backwards now apparently
        int perspective;
        cachedPropertyBlock = new MaterialPropertyBlock();
        MyRenderer.GetPropertyBlock(cachedPropertyBlock);
        if (AnimationUsedForLastAlphaCheck != CurrentAnimation)
        {
           
            Debug.Log("getting texture array for alpha check");
            AnimationUsedForLastAlphaCheck = CurrentAnimation;
            cachedAlpha = (Texture2DArray) cachedPropertyBlock.GetTexture(Alpha);

          
        }
        else
        {
//            Debug.Log("Reusing texture array");
        }
        
        perspective =
            Mathf.Clamp(cachedPropertyBlock.GetInt(Perspective), 0,
                7); //it's actually possible to get back 8 from this which is invalid.
        cachedAlphaPixels = cachedAlpha.GetPixels(perspective);

        var totalWidth = cachedAlpha.width;
        var totalHeight = cachedAlpha.height;
        var frame = cachedPropertyBlock.GetInt(Frame);


        var rows = cachedPropertyBlock.GetInt(Rows);
        var columns = cachedPropertyBlock.GetInt(Columns);
        var frameWidth = (float) totalWidth / columns;
        var frameHeight = (float) totalHeight / rows;
        var numFrames = rows * columns; //includes "blank" frames purposefully

        frame = frame % numFrames; //TODO: consider removing this after resolving bug in exporter

        var columnOfFrame = frame % columns;
        var minXPixel = columnOfFrame / (float) columns * totalWidth;
        var maxXPixel = minXPixel + frameWidth;

        var rowOfFrame = rows - 1 - frame / columns; //frames count from top left to bottom right unlike pixel coords
        var minYPixel = rowOfFrame / (float) rows * totalHeight;
        var maxYPixel = minYPixel + frameHeight;

        var pixelCoord = new Vector2(frameWidth * textureCoord.x + minXPixel,
            frameHeight * textureCoord.y + minYPixel);

        var xPos = Mathf.FloorToInt(pixelCoord.x);
        var yPos = Mathf.FloorToInt(pixelCoord.y);
        var index = yPos * cachedAlpha.width + xPos;
        //.GetPixel( , );
        if (index > cachedAlphaPixels.Count()) Debug.LogError("index out of range!");

        var color = cachedAlphaPixels[index];
        return color.r > .5f;
    }
}