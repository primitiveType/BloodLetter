using UnityEngine;

public class LiquidTypeComponent : MonoBehaviour
{
    [SerializeField] private LiquidType _liquidType;
    public LiquidType LiquidType
    {
        get => _liquidType;
        set => _liquidType = value;
    }
}