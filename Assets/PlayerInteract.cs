using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 1f;

    [SerializeField] private PlayerEvents PlayerEvents;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            var transform1 = transform;
            Ray ray = new Ray(transform1.position, transform1.forward * interactDistance);
            Debug.DrawRay(transform1.position, transform1.forward * interactDistance, Color.blue, 10);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask($"Interactable")))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.Interact();

                PlayerEvents.PlayerInteract(interactable);
            }
        }
    }
}

public interface IInteractable
{
    void Interact();
}