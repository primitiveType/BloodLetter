using System.Collections;
using UnityEngine;

public class PulseScale : MonoBehaviour
{
    [SerializeField] private float MinScale;

    [SerializeField] private float MaxScale;


    [SerializeField] private float TimeToPulse;

    [SerializeField] private float TimeBetweenPulses;

    private void Start()
    {
        StartCoroutine(PulseCR());
    }

    private IEnumerator PulseCR()
    {
        bool reverse = false;
        float t = 0;
        while (true)
        {
       
            var add = ((Time.deltaTime * (1 / TimeToPulse)));
            t += reverse ? -add : add;
            var newScale = Mathf.Lerp(MinScale, MaxScale, t);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            if (t > 1)
            {
                reverse = true;
            }
            else if (t < 0)
            {
                reverse = false;
                yield return new WaitForSeconds(TimeBetweenPulses);
                t = 0;
            }
            else
            {
                yield return null;
            }
        }
    }
}