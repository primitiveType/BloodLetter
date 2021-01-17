using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSource : MonoBehaviour
{
    //TODO:type for blood, oil, etc
    float totalLiquid = 100;
    private LiquidType LiquidType { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        var dragger = other.GetComponent < LiquidDragger>();
        if (!dragger)
        {
            return;
        }
    
        var amountToTake = totalLiquid / 2f;
        totalLiquid -= amountToTake;
        dragger.AddLiquid(amountToTake, LiquidType);
    
        if (totalLiquid < 1f)
        {
            Destroy(this);//stop checking for collisions once we are out of liquid
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LiquidType = GetComponent<LiquidTypeComponent>().LiquidType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
