using UnityEngine;
using UnityEngine.UI;

public abstract class BaseLevelInfoText : MonoBehaviour
{
    protected Text Text { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        string currentLevel = LevelSelectManager.Instance.CurrentSelectedLevel;
        LevelSelectManager.Instance.OnLevelSelected += InstanceOnOnLevelSelected;
        Text = GetComponent<Text>();
        UpdateText(currentLevel);
    }

    private void InstanceOnOnLevelSelected(object sender, LevelSelectedEventArgs args)
    {
        UpdateText(args.LevelName);
    }

    protected abstract void UpdateText(string levelName);
}