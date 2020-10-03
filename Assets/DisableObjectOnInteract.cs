using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectOnInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject toDisable;
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
        toDisable.SetActive(false);
        return true;
    }
}
