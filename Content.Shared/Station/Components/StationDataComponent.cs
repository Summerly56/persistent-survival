using Content.Shared.CrewAssignments.Prototypes;
using Content.Shared.Radio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Station.Components;

/// <summary>
/// Stores core information about a station, namely its config and associated grids.
/// All station entities will have this component.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class StationDataComponent : Component
{
    /// <summary>
    /// The game map prototype, if any, associated with this station.
    /// </summary>
    [DataField]
    public StationConfig? StationConfig;

    /// <summary>
    /// List of all grids this station is part of.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> Grids = new();

    /// <summary>
    /// List of all characters who can access the Station Modification Console
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<string> Owners = new();
    [DataField, AutoNetworkedField]
    public int UID = 0;

    [DataField, AutoNetworkedField]
    public string? StationName;

    [DataField, AutoNetworkedField]
    public int ImportTax = 0;

    [DataField, AutoNetworkedField]
    public int ExportTax = 0;

    [DataField, AutoNetworkedField]
    public int SalesTax = 0;

    [DataField]
    public bool JobNetEnabled = true;

    [DataField]
    public ProtoId<FactionLevelPrototype> Level = "FactionLevel1";

    [DataField]
    public Dictionary<ProtoId<RadioChannelPrototype>, FactionRadioData> RadioData = new()
    {
        { "Common", new FactionRadioData(true) },
        { "Command", new FactionRadioData() },
        { "Engineering", new FactionRadioData() },
        { "Medical", new FactionRadioData() },
        { "Science", new FactionRadioData() },
        { "Security", new FactionRadioData() },
        { "Service", new FactionRadioData() },
        { "Supply", new FactionRadioData() }
    };

    public bool IsOwner(string owner)
    {
        if (Owners.Contains(owner)) return true;
        return false;
    }

    public void RemoveOwner(string owner)
    {
        if (!Owners.Remove(owner)) return;
        Dirty();
    }
    public void AddOwner(string owner)
    {
        if (Owners.Contains(owner)) return;
        Owners.Add(owner);
        Dirty();
    }
}

[DataDefinition]
[Serializable]
[Virtual]
public partial class FactionRadioData
{
    [DataField("_enabled")]
    public bool Enabled = false;
    [DataField("_access")]
    public List<string> Access = new();


    public FactionRadioData(bool enabled = false)
    {
        Enabled = enabled;
    }
}
