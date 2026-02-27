using Content.Shared.Shuttles.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.Events;

[Serializable, NetSerializable]
public sealed class IFFSetDesignationMessage : BoundUserInterfaceMessage
{
    public IFFDesignation Designation;
}
