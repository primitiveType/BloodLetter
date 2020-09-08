using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class AnimationMaterialHelper : MonoBehaviour
{
    private static readonly int Alpha = Shader.PropertyToID("Alpha");
    private static readonly int Perspective = Shader.PropertyToID("Perspective");
    private static readonly int Columns = Shader.PropertyToID("Columns");
    private static readonly int Rows = Shader.PropertyToID("Rows");
    private static readonly int Frame = Shader.PropertyToID("Frame");
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

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();

    private string CurrentAnimation { get; set; }


    private void Awake()
    {
        // MyRenderer.material = new Material(MyRenderer.sharedMaterial);
        if (anchorTransform == null) anchorTransform = materialGameObject.transform;
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
            CurrentAnimation = "";
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

    public void AnimationStarted(string animationName)
    {
        if (animationName != CurrentAnimation)
        {
            CurrentAnimation = animationName;
            // Debug.Log($"{animationName} started!");
            var block = new MaterialPropertyBlock();
            MyRenderer.GetPropertyBlock(block);
            _dictionary.GetPropertyBlock(block, animationName);

            MyRenderer.SetPropertyBlock(block);
            var pxPerMeter = AnimationMaterialDictionary.NumPixelsPerMeter;

            var width = (float) block.GetInt(AnimationMaterialPropertyBlock.FrameWidthProperty) /
                        pxPerMeter;
            var height = (float) block.GetInt(AnimationMaterialPropertyBlock.FrameHeightProperty) /
                         pxPerMeter;
            var offset = height / 2f - height * block.GetFloat(AnimationMaterialPropertyBlock.GroundPositionProperty);
            if (resize)
                anchorTransform.localScale =
                    new Vector3(width, height, 1);

            if (reposition)
            {
                if (anchorTransformCollider) yFudge = anchorTransformCollider.bounds.size.y / 2f;

                anchorTransform.localPosition = new Vector3(0, offset, 0);
            }
        }
    }

    /// <summary>
    ///     returns true if the coordinate is alpha > .5f for the current anim frame
    /// </summary>
    /// <returns></returns>
    public bool QueryAlpha(Vector2 textureCoord)
    {
        int perspective;
        if (AnimationUsedForLastAlphaCheck != CurrentAnimation)
        {
            cachedPropertyBlock = new MaterialPropertyBlock();
            MyRenderer.GetPropertyBlock(cachedPropertyBlock);
            Debug.Log("getting texture array for alpha check");
            AnimationUsedForLastAlphaCheck = CurrentAnimation;
            cachedAlpha = (Texture2DArray) cachedPropertyBlock.GetTexture(Alpha);

            perspective =
                Mathf.Clamp(cachedPropertyBlock.GetInt(Perspective), 0,
                    7); //it's actually possible to get back 8 from this which is invalid.
            cachedAlphaPixels = cachedAlpha.GetPixels(perspective);
        }
        else
        {
            Debug.Log("Reusing texture array");
            perspective =
                Mathf.Clamp(cachedPropertyBlock.GetInt(Perspective), 0,
                    7); //it's actually possible to get back 8 from this which is invalid.
        }


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