using System;
using Bearroll.UltimateDecals;
using UnityEngine;

public class LiquidDecalMaterial : MonoBehaviour
{
    public Material BloodMaterial;

    public Material OilMaterial;

    void Start()
    {
        UltimateDecal myDecal = GetComponent<UltimateDecal>();
        LiquidType type = GetComponent<LiquidTypeComponent>().LiquidType;
        switch (type)    
        {
            case LiquidType.Blood:
                myDecal.material = BloodMaterial;
                break;
            case LiquidType.Oil:
                myDecal.material = OilMaterial;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }        
        
        UD_Manager.UpdateDecal(myDecal); //TODO:!
    }

}

public enum LiquidType
{
    Blood,
    Oil
}
