using Content.Shared.GridControl.Components;
using Content.Shared.GridControl.Systems;

namespace Content.Server.GridControl.Systems;

public sealed partial class BluespaceParkingSystem : SharedBluespaceParkingSystem
{
    private void InitializeEvents()
    {
        SubscribeLocalEvent<BSPParkingTargetComponent, MoveEvent>(OnTargetMove);
        SubscribeLocalEvent<BSPAnchorKeyComponent, ComponentStartup>(UpdateUserInterface);
    }

    private void OnTargetMove(Entity<BSPParkingTargetComponent> ent, ref MoveEvent args)
    {
        CancelRoutine(ent, "Grid has moved.");
    }

}
