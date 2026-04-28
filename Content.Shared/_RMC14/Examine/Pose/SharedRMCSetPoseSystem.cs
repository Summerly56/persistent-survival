using Content.Shared.Actions;
using Content.Shared.Examine;
using Content.Shared.Mobs;

#pragma warning disable IDE1006 // Naming Styles
namespace Content.Shared._RMC14.Examine.Pose;
#pragma warning restore IDE1006 // Naming Styles

public abstract class SharedRMCSetPoseSystem : EntitySystem
{

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RMCSetPoseComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<RMCSetPoseComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnExamine(Entity<RMCSetPoseComponent> ent, ref ExaminedEvent args)
    {
        var comp = ent.Comp;

        if (comp.Pose.Trim() == string.Empty)
            return;

        using (args.PushGroup(nameof(RMCSetPoseComponent)))
        {
            var pose = Loc.GetString("rmc-set-pose-examined", ("ent", ent), ("pose", comp.Pose));
            args.PushMarkup(pose, -5);
        }
    }

    private void OnMobStateChanged(Entity<RMCSetPoseComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Alive)
            return;

        ent.Comp.Pose = string.Empty; // reset the pose on death/crit
        Dirty(ent);
    }
}
