using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObjectAtRaycastHit : MonoBehaviour
{
    [SerializeField] private Camera camera;
    // Start is called before the first frame update

    [SerializeField] private Transform toPoint;

    void Start()
    {
        if (!camera)
            camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            toPoint.LookAt(hit.point);
            // Do something with the object that was hit by the raycast.
        }
    }
}