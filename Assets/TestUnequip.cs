using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnequip : MonoBehaviour
{

    [SerializeField] KeyCode EquipKey;

    [SerializeField] private EquipStatus Equippable;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(EquipKey))
        {
            Toolbox.Instance.EquipThing(Equippable);
        }
    }
}
