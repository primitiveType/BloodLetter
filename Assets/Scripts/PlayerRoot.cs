public class PlayerRoot : ActorRoot
{
    public override EntityType EntityType => EntityType.Player;

    protected override void Awake()
    {
        base.Awake();
        Toolbox.Instance.SetPlayerRoot(this);

    }

    private void Start()
    {
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
        CursorLockManager.Instance.Lock();
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