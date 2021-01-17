using UnityEngine;

public class InteractMaterialSwap : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer materialObject;

    [SerializeField] private Material newMaterial;

    public bool Interact()
    {
        materialObject.material = newMaterial;
        return true;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}