using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
    public static Toolbox Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<Toolbox>();
            }
            return s_instance;
        }
        private set
        {
            s_instance = value;
        }
    }

    public PlayerEvents PlayerEvents { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Transform PlayerHeadTransform { get; private set; }
    
    public PlayerInventory PlayerInventory { get; private set; }

    private EquipStatus CurrentEquip;//will have to make changes if you can later equip multiple things
    private static Toolbox s_instance;

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
        if (CurrentEquip == thing || !thing.CanEquip())
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

    private List<SecretArea> Secrets = new List<SecretArea>();//clear this out between levels
    public void AddSecret(SecretArea secret)
    {
        Secrets.Add(secret);
    }
}