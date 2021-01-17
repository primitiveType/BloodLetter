using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

[RequireComponent(typeof(UltimateDecal))]
public class InteractIndexIncrement : MonoBehaviour, IInteractable
{
    [SerializeField]private UltimateDecal decal;

    [SerializeField] private int incrementAmount;

    private int startIndex;
    // Start is called before the first frame update
    void Start()
    {
        startIndex = decal.atlasIndex;
    }

    public bool Interact()
    {
        Debug.Log("Incrementing decal index");
        decal.atlasIndex += incrementAmount ;
        UD_Manager.UpdateDecal(decal);
        return true;
    }
}
