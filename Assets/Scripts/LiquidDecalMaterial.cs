using System;
using Bearroll.UltimateDecals;
using UnityEngine;

public class LiquidDecalMaterial : MonoBehaviour
{
    public Material BloodMaterial;
    public Material OilMaterial;
    public Material PaintMaterial;

    void Start()
    {
        UltimateDecal myDecal = GetComponent<UltimateDecal>();

        if (SettingsManager.Instance.Settings.PaintballMode)
        {//override visuals for paintball made
            myDecal.material = PaintMaterial;
        }
        else
        {
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
        }

        UD_Manager.UpdateDecal(myDecal); //TODO:!
    }
}

public enum LiquidType
{
    Blood,
    Oil
}