using UnityEngine;

public class LevelBeatenIndicator : MonoBehaviour
{
    void Start()
    {
        LevelInfo currentLevel = LevelSelectManager.Instance.CurrentSelectedLevel;
        LevelSelectManager.Instance.OnLevelSelected += InstanceOnOnLevelSelected;
        gameObject.SetActive(false);
    }

    private void InstanceOnOnLevelSelected(object sender, LevelSelectedEventArgs args)
    {
        gameObject.SetActive(SaveState.Instance.SaveData.BeatenLevels.Contains(args.Level.LevelKey));
    }
}