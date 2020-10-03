using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSource : MonoBehaviour
{
    //TODO:type for blood, oil, etc
    float totalLiquid = 100;
    private void OnTriggerEnter(Collider other)
    {
        var dragger = other.GetComponent < LiquidDragger>();
        if (!dragger)
        {
            return;
        }
    
        var amountToTake = totalLiquid / 2f;
        totalLiquid -= amountToTake;
        dragger.AddLiquid(amountToTake);
    
        if (totalLiquid < 1f)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
