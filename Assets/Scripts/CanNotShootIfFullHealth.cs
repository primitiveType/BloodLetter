using UnityEngine;

public class CanNotShootIfFullHealth : MonoBehaviour, IShootCondition
{
    public bool CanShoot()
    {
        return !Toolbox.Instance.PlayerRoot.Health.IsFullHealth;
    }
}