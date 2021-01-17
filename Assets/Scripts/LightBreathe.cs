using System.Collections;
using UnityEngine;

public class LightBreathe : MonoBehaviour
{
    public float ChangeAmount = 1f;
    public float Frequency = 1f;

    private float startIntensity;

    private Light testLight;

    // Use this for initialization
    private void Start()
    {
        testLight = GetComponent<Light>();
        startIntensity = testLight.intensity;
        StartCoroutine(Flashing());
    }


    private IEnumerator Flashing()
    {
        float t = 0;

        while (true)
        {
            t = 0;
            while (t <= 1f)
            {
                yield return null;
                testLight.intensity = Mathf.Lerp(startIntensity, startIntensity + ChangeAmount, t);
                t += Time.deltaTime * Frequency;
            }

            while (t >= 0)
            {
                yield return null;
                testLight.intensity = Mathf.Lerp(startIntensity, startIntensity + ChangeAmount, t);
                t -= Time.deltaTime * Frequency;
            }
        }
    }
}