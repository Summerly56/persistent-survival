using Content.Shared.CrewAssignments.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.CrewAssignments.Components;

/// <summary>
/// Handles sending order requests to cargo. Doesn't handle orders themselves via shuttle or telepads.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedCrewAssignmentSystem))]
public sealed partial class StationModificationConsoleComponent : Component
{
    /// <summary>
    /// If true, account transfers have no limit and a lower cooldown.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool TransferUnbounded;
}

/// <summary>
/// The behaviour of the cargo order console
/// </summary>
[Serializable, NetSerializable]
public enum StationModificationConsoleMode : byte
{
    /// <summary>
    /// Place orders directly
    /// </summary>
    DirectOrder,
    /// <summary>
    /// Print a slip to be inserted into a DirectOrder console
    /// </summary>
    PrintSlip,
    /// <summary>
    /// Transfers the order to the primary account
    /// </summary>
    SendToPrimary,
}
