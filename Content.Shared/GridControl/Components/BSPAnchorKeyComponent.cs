using Content.Shared.Containers.ItemSlots;
using Content.Shared.GridControl.Systems;
using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.GridControl.Components;

[RegisterComponent]
[Access(typeof(SharedBluespaceParkingSystem))]
public sealed partial class BSPAnchorKeyComponent : Component
{
    public const string PrivilegedIdCardSlotId = "BSPAnchorKey-privilegedId";

    [DataField]
    public ItemSlot PrivilegedIdSlot = new();

    [DataField]
    public TimeSpan? RoutineStartTime { get; set; }


    [DataField]
    public BSPState State { get; set; } = BSPState.Idle;

    public EntityUid? CurrentTarget { get; set; }

    #region Filled Key - When the key is actually holding a parked grid status
    [DataField]
    public string? SavedFilename { get; set; }

    [DataField]
    public DateTime? SavedTimestamp { get; set; }

    [DataField]
    public int? SavedOwnerFaction { get; set; }

    [DataField]
    public string? SavedOwnerPersonal { get; set; }

    [DataField]
    public bool SavedClearOwnership { get; set; } = false;

    [DataField]
    public Vector2? SavedParkedPosition { get; set; }

    [DataField]
    public Box2? SavedParkedBounds { get; set; }

    [DataField]
    public Angle? SavedParkedRotation { get; set; }

    [DataField]
    public int? SavedTileCount { get; set; }

    [DataField]
    public string? SavedGridName { get; set; }
    #endregion
}

/// <summary>
/// Mostly just a tag.
/// </summary>
[RegisterComponent]
[Access(typeof(SharedBluespaceParkingSystem))]
public sealed partial class BSPAnchorKeyUnparkingComponent : Component
{
    public Vector2 Origin { get; set; }
    public Angle Rotation { get; set; }
}

public enum BSPState
{
    /// <summary>
    /// Typical empty anchor key will be in idle state
    /// </summary>
    Idle,

    /// <summary>
    /// Park routine has started
    /// </summary>
    Parking,

    /// <summary>
    /// Grid is parked, the Anchor key now has the grid stored and recoverable at the vicinity park location
    /// </summary>
    Parked,

    /// <summary>
    /// Unpark routine has started
    /// </summary>
    Unparking
}


[Serializable, NetSerializable]
public sealed class BSPAnchorKeyBoundUserInterfaceState : BoundUserInterfaceState
{
    public BSPState State = BSPState.Idle;
    public bool IsAuth = false;
    public bool IsControlled = false;
    public string? OwnerName = null;
    public string? GridName = null;
    public string? IdName = null;

    public int GridTileCount = 0;
    public int GridOwnerTotalTiles = 0;
    public int MaxPersonalClaimTileCount = 0;

    public string? ErrorMessage = null;

    public TimeSpan? RoutineStartTime = null;

    public bool CanAct => GridName != null && IsAuth && ErrorMessage == null;

    public int ParkDelay { get; set; } = -1;
    public int UnparkDelay { get; set; } = -1;
    public Vector2? ParkWorldPosition { get; set; }
    public float UnparkMaxDistance { get; set; } = 0;
    public bool ClearOwnership { get; set; } = false;

    public BSPAnchorKeyBoundUserInterfaceState(
        BSPState state,
        bool isauth, bool isControlled,
        string? ownerName, string? gridname, string? idname,
        int gridTileCount, int gridOwnerTotalTiles, int maxPersonalClaimTileCount, string? errorMessage,
        TimeSpan? routineStartTime, int parkDelay, int unparkDelay,
        Vector2? parkWorldPosition, float unparkMaxDistance, bool clearOwnership
    )
    {
        State = state;
        IsAuth = isauth;
        IsControlled = isControlled;
        OwnerName = ownerName;
        GridName = gridname;
        IdName = idname;
        GridTileCount = gridTileCount;
        GridOwnerTotalTiles = gridOwnerTotalTiles;
        MaxPersonalClaimTileCount = maxPersonalClaimTileCount;
        ErrorMessage = errorMessage;

        RoutineStartTime = routineStartTime;
        ParkDelay = parkDelay;
        UnparkDelay = unparkDelay;
        ParkWorldPosition = parkWorldPosition;
        UnparkMaxDistance = unparkMaxDistance;
        ClearOwnership = clearOwnership;
    }
}

[Serializable, NetSerializable]
public enum BSPAnchorKeyUiKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public enum BSPAnchorKeyVisuals : byte
{
    HasId,
    HasGrid,
    State
}

[Serializable, NetSerializable]
public enum BSPAnchorKeyVisualLayers : byte
{
    Base,
}

[Serializable, NetSerializable]
public sealed class BSPAnchorKeyStartPark : BoundUserInterfaceMessage { }

[Serializable, NetSerializable]
public sealed class BSPAnchorKeyCancel : BoundUserInterfaceMessage { }

[Serializable, NetSerializable]
public sealed class BSPAnchorKeyStartUnpark : BoundUserInterfaceMessage { }

[Serializable, NetSerializable]
public sealed class BSPAnchorKeyToggleClearOwnership : BoundUserInterfaceMessage { }

[Serializable, NetSerializable]
public enum BSPAnchorKeyVisualState : byte
{
    NoIdUnstored,
    NoIdStored,
    IdUnstored,
    IdStored
}
