using System;
using UnityEngine;

public class HideIfNoLevelSelected : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelSelectManager.Instance.OnLevelSelected += InstanceOnOnLevelSelected;
        gameObject.SetActive(!String.IsNullOrWhiteSpace(LevelSelectManager.Instance.CurrentSelectedLevel?.LevelName));
    }

    private void InstanceOnOnLevelSelected(object sender, LevelSelectedEventArgs args)
    {
        gameObject.SetActive(!String.IsNullOrWhiteSpace(args.Level?.LevelName));
    }

}