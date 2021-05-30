using System;
using UnityEngine;

public class SetBloodiedBasedOnCurrentHealth : MonoBehaviour
{
    private Renderer m_MyRenderer;
    private static readonly int BloodAmount = Shader.PropertyToID("BloodAmount");
    private static readonly int BloodColor = Shader.PropertyToID("BloodColor");
    [SerializeField] private GameObject materialGameObject;

    private Renderer MyRenderer =>
        m_MyRenderer != null ? m_MyRenderer : m_MyRenderer = materialGameObject.GetComponent<Renderer>();

    private ActorRoot Root { get; set; }

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            Debug.LogWarning($"Failed to find data provider for {name}.");
        }

        EnemyData data = dataProvider.Data;
        if (!data.CanBleed)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            MyRenderer.GetPropertyBlock(block);
            block.SetColor(BloodColor, Color.black);
            MyRenderer.SetPropertyBlock(block);
        }
    }

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