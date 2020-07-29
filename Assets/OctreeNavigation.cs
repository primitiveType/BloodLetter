using System.Collections;
using System.Collections.Generic;
using CodingEssentials;
using UnityEngine;

public class OctreeNavigation : MonoBehaviour
{
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        // var treew = OctreeManager.Instance.FindSharedRoot(this.GetOrAddComponent<OctreeObject>(), Target);
        StartCoroutine(TestCr());
    }

    private IEnumerator TestCr()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            OctreeManager.Instance.TestPathFinding(transform.position, Target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
