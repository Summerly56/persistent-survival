namespace Content.Shared.Atmos;

/// <summary>
/// Event raised on an entity when it is on a tile containing Chlorine Trifluoride gas.
/// ClF3 is a hypergolic oxidizer that violently corrodes most materials on contact.
/// </summary>
/// <param name="Moles">Amount of ClF3 present on the tile.</param>
/// <param name="Temperature">Current temperature of the gas mixture.</param>
[ByRefEvent]
public readonly record struct TileClF3ExposureEvent(float Moles, float Temperature);
