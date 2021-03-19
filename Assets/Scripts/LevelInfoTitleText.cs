using UnityEngine;

public class LevelInfoTitleText : BaseLevelInfoText
{
    protected override void UpdateText(LevelInfo info)
    {
        Text.text = info == null ? m_EmptyText : info.LevelName;
    }
}