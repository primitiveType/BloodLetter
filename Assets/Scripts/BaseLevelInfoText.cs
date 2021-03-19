using UnityEngine;
using UnityEngine.UI;

public abstract class BaseLevelInfoText : MonoBehaviour
{
    [SerializeField] protected string m_EmptyText;

    protected Text Text { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        LevelInfo currentLevel = LevelSelectManager.Instance.CurrentSelectedLevel;
        LevelSelectManager.Instance.OnLevelSelected += InstanceOnOnLevelSelected;
        Text = GetComponent<Text>();
        UpdateText(currentLevel);
    }

    private void InstanceOnOnLevelSelected(object sender, LevelSelectedEventArgs args)
    {
        UpdateText(args.Level);
    }

    protected abstract void UpdateText(LevelInfo info);
}