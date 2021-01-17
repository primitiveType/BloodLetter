using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCurrentSelectedLevelOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            LevelManager.Instance.StartLevel(LevelSelectManager.Instance.CurrentSelectedLevel.LevelKey));
    }

}