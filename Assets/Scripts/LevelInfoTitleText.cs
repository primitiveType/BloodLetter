public class LevelInfoTitleText : BaseLevelInfoText
{
    protected override void UpdateText(LevelInfo info)
    {
        Text.text = info == null ? null : info.LevelName;
    }
}