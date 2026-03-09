using Content.Shared.Radio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.Events;

/// <summary>
///     Set order in database as approved.
/// </summary>
[Serializable, NetSerializable]
public sealed class StationModificationToggleSpend : BoundUserInterfaceMessage
{
    public int AccessID;

    public StationModificationToggleSpend(int id)
    {
        AccessID = id;
    }
}


[Serializable, NetSerializable]
public sealed class StationModificationToggleGenRec : BoundUserInterfaceMessage
{
    public int AccessID;

    public StationModificationToggleGenRec(int id)
    {
        AccessID = id;
    }
}


[Serializable, NetSerializable]
public sealed class StationModificationToggleClaim : BoundUserInterfaceMessage
{
    public int AccessID;

    public StationModificationToggleClaim(int id)
    {
        AccessID = id;
    }
}

[Serializable, NetSerializable]
public sealed class StationModificationEnableChannel : BoundUserInterfaceMessage
{
    public ProtoId<RadioChannelPrototype> ChannelID;

    public StationModificationEnableChannel(ProtoId<RadioChannelPrototype> id)
    {
        ChannelID = id;
    }
}

[Serializable, NetSerializable]
public sealed class StationModificationDisableChannel : BoundUserInterfaceMessage
{
    public ProtoId<RadioChannelPrototype> ChannelID;

    public StationModificationDisableChannel(ProtoId<RadioChannelPrototype> id)
    {
        ChannelID = id;
    }
}

[Serializable, NetSerializable]
public sealed class StationModificationToggleAssign : BoundUserInterfaceMessage
{
    public int AccessID;

    public StationModificationToggleAssign(int id)
    {
        AccessID = id;
    }
}

[Serializable, NetSerializable]
public sealed class StationModificationJobNetOn : BoundUserInterfaceMessage
{

}
[Serializable, NetSerializable]
public sealed class StationModificationJobNetOff : BoundUserInterfaceMessage
{

}
