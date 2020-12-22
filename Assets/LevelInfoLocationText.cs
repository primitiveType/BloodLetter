public class LevelInfoLocationText : BaseLevelInfoText
{
    protected override void UpdateText(string levelName)
    {
        var info = LevelInfoDatabase.Instance.GetLevelInfo(levelName);
        Text.text = info == null ? null : info.LevelLocation;
    }
}