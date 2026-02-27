using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.Events;

[Serializable, NetSerializable]
public sealed class IFFSetColorMessage : BoundUserInterfaceMessage
{
    public string ColorHex = string.Empty;
}
