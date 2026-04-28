using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.GridControl.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class GridControlConsoleComponent : Component
{
    [DataField]
    public bool Active = true;

    [Serializable, NetSerializable]
    public sealed class GridControlOn : BoundUserInterfaceMessage
    {

        public GridControlOn()
        {
        }
    }

    [Serializable, NetSerializable]
    public sealed class GridControlOff : BoundUserInterfaceMessage
    {

        public GridControlOff()
        {
        }
    }


    [Serializable, NetSerializable]
    public sealed class GridControlConsoleBoundUserInterfaceState : BoundUserInterfaceState
    {
        public bool Active;

        public GridControlConsoleBoundUserInterfaceState(bool active)
        {
            Active = active;
        }
    }

    [Serializable, NetSerializable]
    public enum GridControlConsoleUiKey : byte
    {
        Key,
    }
}
