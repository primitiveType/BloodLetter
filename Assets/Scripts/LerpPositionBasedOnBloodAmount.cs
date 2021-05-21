using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LerpPositionBasedOnBloodAmount : MonoBehaviour
{
    [SerializeField]private Transform m_StartPosition;
    [SerializeField]private Transform m_EndPosition;

    // Start is called before the first frame update
    void Start()
    {
        Toolbox.Instance.PlayerRoot.ActorEvents.OnAmmoChangedEvent += ActorEventsOnOnAmmoChangedEvent;
        UpdatePosition();
    }

    private void ActorEventsOnOnAmmoChangedEvent(object sender, OnAmmoChangedEventArgs args)
    {
        if (args.Type == AmmoType.Blood)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        float lerpValue = Toolbox.Instance.PlayerInventory.GetAmmoAmount(AmmoType.Blood) /
                          Toolbox.Instance.PlayerInventory.GetMaxAmmoAmount(AmmoType.Blood);
        transform.position = Vector3.Lerp(m_StartPosition.position, m_EndPosition.position, lerpValue);
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerRoot.ActorEvents.OnAmmoChangedEvent -= ActorEventsOnOnAmmoChangedEvent;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
