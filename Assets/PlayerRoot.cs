using System;

public class PlayerRoot : ActorRoot
{
    public override EntityType EntityType => EntityType.Player;

    private void Start()
    {
        Toolbox.Instance.SetPlayerRoot(this);
        LevelManager.Instance.LevelEnd += OnLevelEnd;
        LevelManager.Instance.LevelBegin += OnLevelBegin;
        OnLevelBegin();
    }

    private void OnLevelBegin(object sender, LevelBeginEventArgs args)
    {
        OnLevelBegin();
    }

    private void OnLevelBegin()
    {
        Health.Health = SaveState.Instance.SaveData.PlayerHealth;
        Armor.CurrentArmor = SaveState.Instance.SaveData.PlayerArmor;
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.LevelEnd -= OnLevelEnd;
        LevelManager.Instance.LevelBegin -= OnLevelBegin;
    }

    private void OnLevelEnd(object sender, LevelEndEventArgs args)
    {
        if (args.Success)
        {
            SaveState.Instance.SaveData.PlayerHealth = Health.Health;
            SaveState.Instance.SaveData.PlayerArmor = Armor.CurrentArmor;
        }
    }
}