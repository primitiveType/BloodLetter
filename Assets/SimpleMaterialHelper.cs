using UnityEngine;

[ExecuteInEditMode]
public class SimpleMaterialHelper : AnimationMaterialHelper
{
    public string InfoName;

    // Start is called before the first frame update
    private void Start()
    {
        if (!string.IsNullOrEmpty(InfoName)) AnimationStarted(InfoName); //HACKY
    }
}