using Content.Server.GridControl.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.GridControl.Components;

[RegisterComponent, EntityCategory("Spawner")]
[Access(typeof(GridSpawnerSystem))]
public sealed partial class GridSpawnerComponent : Component
{
    [DataField(required: true)]
    public ResPath GridPath { get; set; }

    [DataField]
    public ComponentRegistry? AddComponents { get; set; } = null;

    [DataField]
    public bool StationGrid { get; set; } = false;
}
