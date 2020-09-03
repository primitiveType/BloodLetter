﻿using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;
    [SerializeField] private ActorHealth Health;
    [SerializeField] private Text Text;

    private bool IsDirty { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Events.OnHealthChangedEvent += HealthChanged;
        IsDirty = true;
    }

    private void HealthChanged(object sender, OnHealthChangedEventArgs onHealthChangedEventArgs)
    {
        IsDirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDirty)
        {
            Text.text = $"{Mathf.FloorToInt(Health.Health)} / {Mathf.FloorToInt(Health.MaxHealth)}";
        }
    }

    private void OnDestroy()
    {
        Events.OnHealthChangedEvent -= HealthChanged;
    }
}
