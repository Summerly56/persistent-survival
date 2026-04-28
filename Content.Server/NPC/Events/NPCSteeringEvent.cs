using Content.Server.NPC.Components;
using System.Numerics;

namespace Content.Server.NPC.Events;

/// <summary>
/// Raised directed on an NPC when steering.
/// </summary>
[ByRefEvent]
public readonly record struct NPCSteeringEvent(
    NPCSteeringComponent Steering,
    TransformComponent Transform,
    Vector2 WorldPosition,
    Angle OffsetRotation);
