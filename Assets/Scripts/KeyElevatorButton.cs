using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyElevatorButton : ElevatorButton
{
   [SerializeField] private KeyType KeyType;
   public override bool Interact()
   {
      if (Toolbox.Instance.PlayerInventory.HasKey(KeyType))
      {
         return base.Interact();
      }

      return false;
   }
}
