using System.Collections.Generic;
using UnityEngine;

public class LineRendererPointSync : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private List<Transform> Points;

    // Start is called before the first frame update
    private void Start()
    {
        Application.onBeforeRender += ApplicationOnonBeforeRender;
    }

    private void ApplicationOnonBeforeRender()
    {
        var index = 0;
        var points = new Vector3[Points.Count];
        for (var i = 0; i < Points.Count; i++) points[i] = Points[i].position;

        _lineRenderer.SetPositions(points);
    }

    private void OnDestroy()
    {
        Application.onBeforeRender -= ApplicationOnonBeforeRender;
    }
}