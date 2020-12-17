using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PreloadAssets : MonoBehaviour
{
    public List<AnimationMaterialDictionary> Dictionaries;

    public float progress;

    private void Awake()
    {
        StartCoroutine(PreloadCr());
    }

    // Start is called before the first frame update
    IEnumerator PreloadCr()
    {
        Time.timeScale = 0;
        List<Task> loadingTasks = new List<Task>();
        // foreach (var dictionary in Dictionaries)
        // {
        //     foreach (AnimationMaterialPropertyBlock block in dictionary.PropertyBlocks)
        //     {
        //         loadingTasks.Add(block.GetDiffuseMap());
        //         loadingTasks.Add(block.GetNormalMap());
        //         loadingTasks.Add(block.GetAlphaMap());
        //     }
        // }

        var allTask = Task.WhenAll(loadingTasks);
        while (!allTask.IsCompleted)
        {
            yield return null;
            int count = 0;
            foreach (var t in loadingTasks)
            {
                if (t.IsCompleted) count++;
            }

            progress = loadingTasks.Count / (float)count;
            
        }

        progress = 1f;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
