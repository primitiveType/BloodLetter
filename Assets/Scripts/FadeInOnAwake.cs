using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnAwake : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
