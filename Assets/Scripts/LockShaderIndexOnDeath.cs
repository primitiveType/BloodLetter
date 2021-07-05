using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockShaderIndexOnDeath : MonoBehaviour
{
    [SerializeField] private ActorRoot root;

    [SerializeField] private ChangeShaderIndex indexChanger;

    [SerializeField] private int index;

    // Start is called before the first frame update
    void Start()
    {
        root.ActorEvents.OnDeathEvent += OnDeathEvent;
    }

    private void OnDeathEvent(object sender, OnDeathEventArgs args)
    {
        indexChanger.IndexOverride = index;
        indexChanger.enabled = false;
    }

    private void OnDestroy()
    {
        root.ActorEvents.OnDeathEvent -= OnDeathEvent;
    }
}
