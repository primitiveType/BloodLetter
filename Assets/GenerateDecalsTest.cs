using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

public class GenerateDecalsTest : MonoBehaviour
{
    public GameObject prefab;

    public int rows;

    public int cols;

    public float spacing;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var go = Instantiate(prefab, transform.position + new Vector3(i * spacing, .1f, j * spacing), transform.rotation,
                    transform);
                var decal = go.GetComponentInChildren<UltimateDecal>(true);
                var random = go.GetComponentInChildren<RandomDecalAtlas>(true);
                random.loops = true;
                decal.atlasIndex = (i * cols) + j;
                UD_Manager.UpdateDecal(decal);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}