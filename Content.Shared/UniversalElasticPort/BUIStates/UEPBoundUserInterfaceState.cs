using Content.Shared.Atmos;
using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.MCTN.BUIStates;

[Serializable, NetSerializable, Virtual]
public class MCTNAvailableConnection
{
    public NetEntity Entity;
    public string Name = string.Empty;
    public Vector2 Position;
    public float Distance;
    public bool Occupied = false;
}

[Serializable, NetSerializable]
public sealed class MCTNCurrentConnection : MCTNAvailableConnection
{
    // Unsure if more information for the current connection will be needed.
}

[Serializable, NetSerializable]
public abstract class MCTNBasePlugState
{
    public string Identifier { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public bool IsNetworked { get; set; } = false;
    public bool IsRemoteEnabled { get; set; } = false;
}

[Serializable, NetSerializable]
public abstract class MCTNCounterpartState;

[Serializable, NetSerializable]
public sealed class MCTNPowerState : MCTNCounterpartState
{
    // @TODO: Maybe power supply/demand
    public float CombinedLoad { get; set; }
    public float CombinedSupply { get; set; }
    public float CombinedMaxSupply { get; set; }
}

[Serializable, NetSerializable]
public sealed class MCTNPipeState : MCTNCounterpartState
{
    public GasMixture GasMix { get; set; } = GasMixture.SpaceGas;
}

[Serializable, NetSerializable]
public abstract class MCTNPlugStateCounterparts<T>(T local, T remote) : MCTNBasePlugState where T : MCTNCounterpartState
{
    public T LocalState { get; set; } = local;
    public T RemoteState { get; set; } = remote;
}

[Serializable, NetSerializable]
public sealed class MCTNPowerPlugState(MCTNPowerState local, MCTNPowerState remote) : MCTNPlugStateCounterparts<MCTNPowerState>(local, remote) { }
[Serializable, NetSerializable]
public sealed class MCTNPipePlugState(MCTNPipeState local, MCTNPipeState remote) : MCTNPlugStateCounterparts<MCTNPipeState>(local, remote) { }

[Serializable, NetSerializable]
public sealed class MCTNBoundUserInterfaceState : BoundUserInterfaceState
{
    public float MaxRange;
    public List<MCTNAvailableConnection> AvailableConnections = [];
    public MCTNCurrentConnection? CurrentConnection = null;
    public Dictionary<string, MCTNBasePlugState> PlugStates = new();
}

[Serializable, NetSerializable]
public enum MCTNConsoleUiKey : byte
{
    Key
}

#region Messages
[Serializable, NetSerializable]
public sealed class MCTNConnectMessage(NetEntity target) : BoundUserInterfaceMessage
{
    public NetEntity Target = target;
}

[Serializable, NetSerializable]
public sealed class MCTNDisconnectMessage() : BoundUserInterfaceMessage { }


[Serializable, NetSerializable]
public sealed class MCTNTogglePlugMessage(string identifier) : BoundUserInterfaceMessage
{
    public string Identifier = identifier;
}
#endregion
