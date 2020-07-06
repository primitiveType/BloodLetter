using System;
using System.Collections;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
    
    public static Toolbox Instance { get; private set; }
    public PlayerEvents PlayerEvents { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Transform PlayerHeadTransform { get; private set; }
    
    public PlayerInventory PlayerInventory { get; private set; }

    private EquipStatus CurrentEquip;//will have to make changes if you can later equip multiple things

    private void Awake()
    {
        Instance = this;
    }

    public void EquipThing(EquipStatus thing)
    {
        Instance.StartCoroutine(EquipThingCR(thing));
    }

    private IEnumerator EquipThingCR(EquipStatus thing)
    {
        if (CurrentEquip == thing)
        {
            yield break;
        }
        if (CurrentEquip != null)
        {
            yield return StartCoroutine(CurrentEquip.UnEquip());
        }

        yield return StartCoroutine(thing.Equip());
        CurrentEquip = thing;
    }
    
    public void SetPlayerEvents(PlayerEvents events)
    {
        PlayerEvents = events;
    }

    public void SetPlayerTransform(Transform transform)
    {
        PlayerTransform = transform;
    }
    
    public void SetPlayerHeadTransform(Transform transform)
    {
        PlayerHeadTransform = transform;
    }

    public void SetPlayerInventory(PlayerInventory inventory)
    {
        PlayerInventory = inventory;
    }
}