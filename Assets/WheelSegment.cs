
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelSegment : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    private static readonly int MinMaxAngle = Shader.PropertyToID("AngleMinMax");
    private Material Material { get; set; }

    private Image Renderer { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<Image>();
        Material = new Material(Renderer.material);
        Renderer.material = Material;
    }

    // Update is called once per frame
    void Update()
    {
        Material.SetVector(MinMaxAngle, new Vector4(minAngle, maxAngle,0,0));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
    }
}
