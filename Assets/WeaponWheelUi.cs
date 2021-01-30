using System;
using ECM.Components;
using UnityEngine;

public class WeaponWheelUi : MonoBehaviour
{
    private UltimateRadialMenu Menu { get; set; }

    private MouseLook MouseLook { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MouseLook = Toolbox.Instance.PlayerRoot.GetComponentInChildren<MouseLook>();
        Menu = UltimateRadialMenu.GetUltimateRadialMenu("WeaponWheel");
        Menu.OnRadialMenuEnabled += OnRadialMenuEnabled;
        Menu.OnRadialMenuDisabled += OnRadialMenuDisabled;
    }

    private void OnRadialMenuDisabled()
    {
        MouseLook.enabled = true;
    }

    private void OnRadialMenuEnabled()
    {
        MouseLook.enabled = false;
    }

    private void OnDestroy()
    {
        MouseLook.enabled = true;
        Menu.OnRadialMenuEnabled -= OnRadialMenuEnabled;
        Menu.OnRadialMenuDisabled -= OnRadialMenuDisabled;
    }
}