using Content.Shared.Access.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Access.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedIdCardConsoleSystem))]
public sealed partial class IdPrinterConsoleComponent : Component
{
    [Serializable, NetSerializable]
    public sealed class PrintID : BoundUserInterfaceMessage
    {

        public PrintID()
        {
        }
    }


    [Serializable, NetSerializable]
    public sealed class IdPrinterConsoleBoundUserInterfaceState : BoundUserInterfaceState
    {


        public IdPrinterConsoleBoundUserInterfaceState()
        {

        }
    }

    [Serializable, NetSerializable]
    public enum IdPrinterConsoleUiKey : byte
    {
        Key,
    }
}
