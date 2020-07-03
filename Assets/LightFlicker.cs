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

    // Update is called once per frame
    void Update () {

    }


    IEnumerator Flashing(){
        while (true) {
            yield return new WaitForSeconds (Random.Range(minFlickerTime,maxFlickerTime));
            testLight.intensity = Random.Range(startIntensity - FlickerAmount, startIntensity + FlickerAmount);
            //testLight.enabled = !testLight.enabled;

        }
    }
}