using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleMaterialHelper : AnimationMaterialHelper
{
    public string InfoName;
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(InfoName))
        {
            AnimationStarted(InfoName); //HACKY
        }
    }

   
}
