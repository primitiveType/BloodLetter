﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyElevatorButton : ElevatorButton
{
   [SerializeField] private KeyType KeyType;
   public override void Interact()
   {
      if (Toolbox.PlayerInventory.HasKey(KeyType))
      {
         base.Interact();
      }
   }
}