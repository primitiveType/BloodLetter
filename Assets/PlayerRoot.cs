using System;

public class PlayerRoot : ActorRoot
{
    public override EntityType EntityType => EntityType.Player;

    private void Start()
    {
        Toolbox.Instance.SetPlayerRoot(this);
    }
}