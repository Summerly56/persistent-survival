using Content.Server.Atmos.Piping.Components;
using Content.Server.MCTN.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Nodes;
using Content.Shared.Coordinates;
using Content.Shared.NodeContainer.NodeGroups;
using Content.Shared.Physics;
using Robust.Shared.Utility;
using System.Linq;
using System.Numerics;

namespace Content.Server.MCTN.Systems;

public sealed partial class MCTNSystem : EntitySystem
{
    /// <summary>
    /// How much will cable/hose joints be offset from the center of each port - currently random based on this value.
    /// </summary>
    private const float MaxJointOffset = 1f / 6f;

    private Dictionary<EntityUid, Dictionary<string, EntityUid>> _tethersByConnection = new();

    private void InitializeTethers()
    {
        SubscribeLocalEvent<MCTNTetherComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<MCTNTetherComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnStartup(Entity<MCTNTetherComponent> ent, ref ComponentStartup args)
    {
        if (ent.Comp.Connection == default) return; // Initializing
        if (
            !ent.Comp.Connection.Valid ||
            TerminatingOrDeleted(ent.Comp.Connection) ||
            !_tethersByConnection.ContainsKey(ent.Comp.Connection) ||
            !_tethersByConnection[ent.Comp.Connection].ContainsKey(ent.Comp.NodeIdentifier) ||
            _tethersByConnection[ent.Comp.Connection][ent.Comp.NodeIdentifier] != ent.Owner
        )
        {
            QueueDel(ent);
            return;
        }
    }

    private void OnShutdown(Entity<MCTNTetherComponent> ent, ref ComponentShutdown args)
    {
        if (
            _tethersByConnection.ContainsKey(ent.Comp.Connection) &&
            _tethersByConnection[ent.Comp.Connection].ContainsKey(ent.Comp.NodeIdentifier) &&
            _tethersByConnection[ent.Comp.Connection][ent.Comp.NodeIdentifier] == ent.Owner
        )
        {
            _tethersByConnection[ent.Comp.Connection].Remove(ent.Comp.NodeIdentifier);
        }
    }

    public void SetupTethering(Entity<MCTNConnectionComponent> entity)
    {
        _tethersByConnection.Add(entity, new());
        UpdateTethering(entity);
    }

    public void BreakdownTethering(Entity<MCTNConnectionComponent> entity)
    {
        if (!_tethersByConnection.TryGetValue(entity, out var nodeTethers)) return;
        foreach (var uid in nodeTethers.Values)
            QueueDel(uid);
        nodeTethers.Clear();
        _tethersByConnection.Remove(entity);
    }

    public void UpdateTethering(Entity<MCTNConnectionComponent> entity)
    {
        if (!_tethersByConnection.TryGetValue(entity, out var nodeTethers))
            return;

        if (!TryComp<MCTNComponent>(entity.Comp.AnchorA, out var mctnA)) return;
        if (!TryComp<MCTNComponent>(entity.Comp.AnchorB, out var mctnB)) return;
        Entity<MCTNComponent> anchorA = (entity.Comp.AnchorA, mctnA);
        Entity<MCTNComponent> anchorB = (entity.Comp.AnchorB, mctnB);

        foreach (var key in mctnA.EnabledPlugs.Keys.Concat(mctnB.EnabledPlugs.Keys).Distinct())
        {
            if (mctnA.EnabledPlugs.TryGetValue(key, out var value1) && mctnB.EnabledPlugs.TryGetValue(key, out var value2) && value1 && value2)
            {
                EnsureCreateNodeTether(nodeTethers, entity, anchorA, anchorB, key);
            }
            else
                EnsureDeleteNodeTether(nodeTethers, entity, key);
        }
    }

    public void UpdateTethering(Entity<MCTNComponent> entity)
    {
        if (IsConnected(entity) && TryComp<MCTNConnectionComponent>(entity.Comp.Connection, out var conn))
            UpdateTethering((entity.Comp.Connection.GetValueOrDefault(), conn));
    }

    private void EnsureCreateNodeTether(Dictionary<string, EntityUid> nodeTethers, Entity<MCTNConnectionComponent> entity, Entity<MCTNComponent> anchorA, Entity<MCTNComponent> anchorB, string key)
    {
        EntityUid tetherUid;
        if (!nodeTethers.TryGetValue(key, out var value))
        {
            tetherUid = SpawnAttachedTo("MCTNTetherStub", anchorA.Owner.ToCoordinates());
            var tetherComp = EnsureComp<MCTNTetherComponent>(tetherUid);
            tetherComp.Connection = entity;
            tetherComp.NodeIdentifier = key;
            nodeTethers.Add(key, tetherUid);
        }
        else
            tetherUid = value;

        if (!HasComp<JointVisualsComponent>(tetherUid))
        {
            var visuals = EnsureComp<JointVisualsComponent>(tetherUid);
            visuals.OffsetA = new Vector2(_random.NextFloat(-MaxJointOffset, MaxJointOffset), _random.NextFloat(-MaxJointOffset, MaxJointOffset));
            visuals.OffsetB = new Vector2(_random.NextFloat(-MaxJointOffset, MaxJointOffset), _random.NextFloat(-MaxJointOffset, MaxJointOffset));
            visuals.OffsetRotationMode = JointOffsetRotationMode.TowardsTarget;
            visuals.Target = anchorB;
            visuals.Sprite = new SpriteSpecifier.Rsi(new ResPath("Structures/Power/Cables/lv_cable.rsi"), "lvcable_3");

            // Find sprite.
            var node = GetPlugNode(anchorA, key);

            var physicalNode = node?.NodeGroup?.Nodes.FirstOrDefault(x => x is not PortPipeNode && x is not CableDeviceNode);
            if (physicalNode != null && physicalNode is PipeNode pipe && TryComp<AtmosPipeColorComponent>(pipe.Owner, out var pipeColor))
            {
                visuals.Sprite = new SpriteSpecifier.Rsi(new ResPath("Structures/Piping/Atmospherics/pipe.rsi"), "pipeStraight");
                visuals.Modulate = pipeColor.Color;
            }
            else if (physicalNode != null && physicalNode is CableNode cable)
            {
                if (cable.NodeGroup is BaseNodeGroup baseGroup)
                    visuals.Modulate = NodeGroupSystem.CalcNodeGroupColor(baseGroup);
                switch (cable.NodeGroupID)
                {
                    case NodeGroupID.HVPower:
                        {
                            visuals.Sprite = new SpriteSpecifier.Rsi(new ResPath("Structures/Power/Cables/hv_cable.rsi"), "hvcable_3");
                            break;
                        }
                    case NodeGroupID.MVPower:
                        {
                            visuals.Sprite = new SpriteSpecifier.Rsi(new ResPath("Structures/Power/Cables/mv_cable.rsi"), "mvcable_3");
                            visuals.SpriteOverlay = new SpriteSpecifier.Rsi(new ResPath("Structures/Power/Cables/mv_cable.rsi"), "mvstripes_3");
                            break;
                        }
                }
            }
            else
            {
                // Too early to load a tether it seems - next tick should be ok.
                QueueDel(tetherUid);
                nodeTethers.Remove(key);
                return;
            }
            Dirty(tetherUid, visuals);
        }
    }

    private void EnsureDeleteNodeTether(Dictionary<string, EntityUid> nodeTethers, Entity<MCTNConnectionComponent> entity, string key)
    {
        if (nodeTethers.TryGetValue(key, out var tetherUid))
        {
            nodeTethers.Remove(key);
            if (!TerminatingOrDeleted(tetherUid))
                QueueDel(tetherUid);
        }
    }
}
