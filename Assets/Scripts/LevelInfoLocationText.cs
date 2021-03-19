public class LevelInfoLocationText : BaseLevelInfoText
{
    protected override void UpdateText(LevelInfo info)
    {
        Text.text = info == null ? m_EmptyText : info.LevelLocation;
    }
}