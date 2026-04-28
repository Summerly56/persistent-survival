using Robust.Shared.Serialization;

namespace Content.Shared.AcceptDeath;

[Serializable, NetSerializable]
public enum AcceptDeathUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class AcceptDeathUpdateState : BoundUserInterfaceState
{
    public TimeSpan acceptDeathCooldown;
    public TimeSpan sosCooldown;
    public AcceptDeathUpdateState(TimeSpan acceptDeathCooldown, TimeSpan sosCooldown)
    {
        this.acceptDeathCooldown = acceptDeathCooldown;
        this.sosCooldown = sosCooldown;
    }
}

[Serializable, NetSerializable]
public sealed class AcceptDeathFinalizeMessage : BoundUserInterfaceMessage
{

}


[Serializable, NetSerializable]
public sealed class AcceptDeathSOSMessage : BoundUserInterfaceMessage
{

}

