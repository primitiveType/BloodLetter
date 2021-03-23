public class PlayerRoot : ActorRoot
{
    public override EntityType EntityType => EntityType.Player;

    public override bool IsGrounded => MovementHandler.IsGrounded;

    private IMovementHandler MovementHandler { get; set; }

    private PlayerBlood PlayerBlood { get; set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerBlood = GetComponentInChildren<PlayerBlood>();
        Toolbox.Instance.SetPlayerRoot(this);
        MovementHandler = GetComponent<IMovementHandler>();
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
        Health.Health = Health.MaxHealth;// SaveState.Instance.SaveData.PlayerHealth;
        Armor.CurrentArmor = 0;// SaveState.Instance.SaveData.PlayerArmor;
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