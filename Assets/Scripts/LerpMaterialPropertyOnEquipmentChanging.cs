using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMaterialPropertyOnEquipmentChanging : MonoBehaviour
{
    private PlayerRoot PlayerRoot { get; set; }

    public WeaponId WeaponId
    {
        get => m_WeaponId;
        set => m_WeaponId = value;
    }

    [SerializeField] private WeaponId m_WeaponId;
    [SerializeField] private string m_MaterialProperty = "Alpha";

    private Coroutine LerpCoroutine { get; set; }

    public string MaterialProperty => m_MaterialProperty;
    private List<Material> Materials { get; } = new List<Material>();
    [SerializeField] private float m_TimeToLerp = .5f;

    private void Start()
    {
        PlayerRoot = GetComponentInParent<PlayerRoot>();
        PlayerRoot.ActorEvents.OnEquippedWeaponChangedEvent += OnWeaponChanged;
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                Materials.Add(material);
            }
        }
        
        if (Toolbox.Instance.PlayerInventory.IsEquipped(WeaponId, PlayerInventory.EquipmentSlot.RightHand) ||
            Toolbox.Instance.PlayerInventory.IsEquipped(WeaponId, PlayerInventory.EquipmentSlot.LeftHand))
        {
            UpdateMaterials(WeaponId);
        }
        else
        {
            UpdateMaterials();
        }
    }

    private void OnWeaponChanged(object sender, OnEquippedWeaponChangedEventArgs args)
    {
        var weapon = args.NewValue;
        UpdateMaterials(weapon);
    }

    private void UpdateMaterials(WeaponId weapon)
    {
        if (weapon.HasFlag(WeaponId))
        {
            Lerp(1);
        }
        else
        {
            Lerp(0);
        }
    }

    private void Lerp(float targetValue)
    {
        if (LerpCoroutine != null)
        {
            StopCoroutine(LerpCoroutine);
        }

        LerpCoroutine = StartCoroutine(LerpOverTime(targetValue));
    }

    private float currentValue;

    private IEnumerator LerpOverTime(float targetValue)
    {
        float t = 0;
        float startValue = currentValue;

        while (t < m_TimeToLerp)
        {
            t += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, targetValue, t / m_TimeToLerp);
            UpdateMaterials();
            yield return null;
        }

        currentValue = targetValue;
        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        foreach (var material in Materials)
        {
            material.SetFloat(MaterialProperty, currentValue);
        }
    }
}