using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMaterialHelper : AnimationMaterialHelper
{
    public string InfoName;
    // Start is called before the first frame update
    void Start()
    {
        AnimationStarted(InfoName);//HACKY
    }

   
}
