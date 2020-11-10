using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithChildren : MonoBehaviour , IInteractable
{
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
        var interactables = GetComponentsInChildren<IInteractable>();
        bool didit = false;
        foreach (var interactable in interactables)
        {
            if (interactable == this)
                continue;
            didit |= interactable.Interact();
        }

        return didit;
    }
}

