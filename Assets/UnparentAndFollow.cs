using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnparentAndFollow : MonoBehaviour
{
    [SerializeField]private Transform toFollow;
    [SerializeField]private UnityEvent AfterUnparentAction;
    private Transform myTransform;
    void Start()
    {
        StartCoroutine(ReparentCR());
    }

    private IEnumerator ReparentCR()
    {
        yield return null;
        
        myTransform = transform;
        toFollow = myTransform.parent;
        myTransform.parent = null;
        AfterUnparentAction?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (toFollow)
        {
            myTransform.position = toFollow.position;
            myTransform.rotation = toFollow.rotation;
            myTransform.localScale = toFollow.lossyScale;
        }
    }
}
