using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnequip : MonoBehaviour
{
    [SerializeField] KeyCode EquipKey;

    [SerializeField] private EquipStatus Equippable;
    [SerializeField] private bool equipOnStart;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(EquipKey))
        {
            Toolbox.Instance.EquipThing(Equippable);
        }
    }

    void Start()
    {
        if (equipOnStart)
        {
            Toolbox.Instance.EquipThing(Equippable);
        }
        else
        {
            Equippable.UnEquipInstant();
        }
    }
}