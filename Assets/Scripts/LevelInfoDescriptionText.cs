﻿public class LevelInfoDescriptionText : BaseLevelInfoText
{
    protected override void UpdateText(LevelInfo info)
    {
        Text.text = info == null ? m_EmptyText : info.LevelDescription;
    }
}