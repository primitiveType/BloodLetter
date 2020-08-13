using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    Light testLight;

    public float minFlickerTime = 0.1f;
    public float maxFlickerTime = 0.04f;

    public float FlickerAmount = 1f;

    private float startIntensity;
    // Use this for initialization
    void Start () {
        testLight = GetComponent<Light> ();
        startIntensity = testLight.intensity;
        StartCoroutine (Flashing ());
    }

    IEnumerator Flashing(){
        while (true) {
            yield return new WaitForSeconds (Random.Range(minFlickerTime,maxFlickerTime));
            testLight.intensity = Random.Range(startIntensity - FlickerAmount, startIntensity + FlickerAmount);
            //testLight.enabled = !testLight.enabled;

        }
    }
}
//
// public class LightBreathe : MonoBehaviour {
//
//     Light testLight;
//
//     public float minFlickerTime = 0.1f;
//     public float maxFlickerTime = 0.04f;
//
//     public float BreatheAmount = 1f;
//
//     private float distance;
//     
//     private float startIntensity;
//     // Use this for initialization
//     void Start () {
//         testLight = GetComponent<Light> ();
//         distance = BreatheAmount / 2f;
//         startIntensity = testLight.intensity;
//         testLight.intensity = startIntensity - distance;
//         StartCoroutine (Breathe ());
//     }
//
//     IEnumerator Breathe(){
//         float t = 0;
//
//         while (true)
//         {
//             yield return null;
//             t += Time.deltaTime;
//             testLight.intensity= EasingFunction.EaseInOutBack(startIntensity, startIntensity + distance, t);
//         }
//     }
// }