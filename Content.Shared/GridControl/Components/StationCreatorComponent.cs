using Content.Shared.Containers.ItemSlots;
using Content.Shared.GridControl.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.GridControl.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedGridConfigSystem))]
public sealed partial class StationCreatorComponent : Component
{
    public static string PrivilegedIdCardSlotId = "StationCreator-privilegedId";

    [DataField]
    public ItemSlot PrivilegedIdSlot = new();

    [DataField]
    public int? ConnectedStation = null;

    [DataField]
    public bool PersonalMode = false;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float DoAfter = 5f;



    [Serializable, NetSerializable]
    public sealed class StationCreatorBoundUserInterfaceState : BoundUserInterfaceState
    {
        public bool IdPresent = false;
        public string? IdName = null;
        public string? RealName = null;


        public StationCreatorBoundUserInterfaceState(bool idpresent, string? idname, string? realname)
        {
            IdPresent = idpresent;
            IdName = idname;
            RealName = realname;
        }
    }

    [Serializable, NetSerializable]
    public enum StationCreatorUiKey : byte
    {
        Key,
    }

    [Serializable, NetSerializable]
    public sealed class StationCreatorFinish : BoundUserInterfaceMessage
    {
        public string? StationName;
        public StationCreatorFinish(string? stationName)
        {
            StationName = stationName;
        }
    }


}
