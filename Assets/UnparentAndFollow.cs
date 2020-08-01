﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentAndFollow : MonoBehaviour
{
    private Transform toFollow;
    private Transform myTransform;
    void Awake()
    {
        myTransform = transform;
        toFollow = myTransform.parent;
        myTransform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = toFollow.position;
        myTransform.rotation = toFollow.rotation;
        myTransform.localScale = toFollow.lossyScale;
    }
}