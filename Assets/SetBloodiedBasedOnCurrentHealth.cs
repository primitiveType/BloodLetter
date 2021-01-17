using UnityEngine;

public class SetBloodiedBasedOnCurrentHealth : MonoBehaviour
{
    private Renderer m_MyRenderer;
    private static readonly int BloodAmount = Shader.PropertyToID("BloodAmount");
    [SerializeField] private GameObject materialGameObject;

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();

    private ActorRoot Root { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Root = GetComponentInChildren<ActorRoot>();
        Root.ActorEvents.OnHealthChangedEvent += OnHealthChanged;
    }

    private void OnHealthChanged(object sender, OnHealthChangedEventArgs args)
    {
        var blood = Mathf.Clamp01(1 - (Root.Health.Health / Root.Health.MaxHealth));

        MaterialPropertyBlock block = new MaterialPropertyBlock();

        MyRenderer.GetPropertyBlock(block);
        block.SetFloat(BloodAmount, blood);
        MyRenderer.SetPropertyBlock(block);
    }

    private void OnDestroy()
    {
        Root.ActorEvents.OnHealthChangedEvent -= OnHealthChanged;
    }
}