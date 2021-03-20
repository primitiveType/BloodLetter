using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;
    [SerializeField] private ActorHealth Health;
    [SerializeField] private Text Text;
    [SerializeField] private Image Image;
    [SerializeField] private Image OverhealImage;

    private bool IsDirty { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        Events.OnHealthChangedEvent += HealthChanged;
        IsDirty = true;
    }

    private void HealthChanged(object sender, OnHealthChangedEventArgs onHealthChangedEventArgs)
    {
        IsDirty = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsDirty)
        {
            Text.text = $"{Mathf.FloorToInt(Health.Health)} / {Mathf.FloorToInt(Health.MaxHealth)}";
            Image.fillAmount = Health.Health / Health.MaxHealth;
            float overhealMax = Health.OverhealMaxHealth - Health.MaxHealth;
            float overhealAmount = Health.Health - Health.MaxHealth; 
            OverhealImage.fillAmount = overhealAmount / overhealMax;
        }
    }

    private void OnDestroy()
    {
        Events.OnHealthChangedEvent -= HealthChanged;
    }
}