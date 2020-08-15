using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererPointSync : MonoBehaviour
{
    [SerializeField] private List<Transform> Points;
    [SerializeField] private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
     Application.onBeforeRender += ApplicationOnonBeforeRender;   
    }

    private void ApplicationOnonBeforeRender()
    {
        int index = 0;
        Vector3[] points = new Vector3[Points.Count];
        for (int i = 0; i < Points.Count; i++)
        {
            points[i] = Points[i].position;
        }
        
        _lineRenderer.SetPositions(points);
    }

    private void OnDestroy()
    {
        Application.onBeforeRender -= ApplicationOnonBeforeRender;
    }
}
