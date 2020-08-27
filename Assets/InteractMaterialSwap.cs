using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMaterialSwap : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer materialObject;

    [SerializeField] private Material newMaterial;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool Interact()
    {
        materialObject.material = newMaterial;
        return true;
    }
}