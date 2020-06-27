using UnityEngine;

public class Toolbox : MonoBehaviour
{
    public static PlayerEvents PlayerEvents { get; private set; }
    public static Transform PlayerTransform { get; private set; }
    
    public static PlayerInventory PlayerInventory { get; private set; }

    public static void SetPlayerEvents(PlayerEvents events)
    {
        PlayerEvents = events;
    }

    public static void SetPlayerTransform(Transform transform)
    {
        PlayerTransform = transform;
    }

    public static void SetPlayerInventory(PlayerInventory inventory)
    {
        PlayerInventory = inventory;
    }
}