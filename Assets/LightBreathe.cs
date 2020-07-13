using System.Collections;
using UnityEngine;

public class LightBreathe : MonoBehaviour
{
    
    Light testLight;

    public float ChangeAmount = 1f;
    public float Frequency = 1f;

    private float startIntensity;
    // Use this for initialization
    void Start () {
        testLight = GetComponent<Light> ();
        startIntensity = testLight.intensity;
        StartCoroutine (Flashing ());
    }



    IEnumerator Flashing()
    {
        float t = 0;

        while (true)
        {
            t = 0;
            while (t <= 1f)
            {
                yield return null;
                testLight.intensity = Mathf.Lerp(startIntensity, startIntensity + (ChangeAmount), t);
                t += Time.deltaTime * Frequency;
            }

            while (t >= 0)
            {
                yield return null;
                testLight.intensity = Mathf.Lerp(startIntensity, startIntensity + (ChangeAmount), t);
                t -= Time.deltaTime * Frequency;
            }
        }
    }
}