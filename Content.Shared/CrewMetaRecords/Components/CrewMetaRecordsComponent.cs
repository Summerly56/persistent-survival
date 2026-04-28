using Content.Shared.CrewAssignments.Prototypes;
using Content.Shared.CrewAssignments.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.CrewMetaRecords;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class CrewMetaRecordsComponent : Component
{
    [DataField]
    public string SectorStatus = "";
    [DataField]
    public int SectorChaos = 0;
    [DataField]
    public int NextObjectiveID = 1;
    [DataField]
    public int NextCodexID = 1;
    [DataField]
    public List<WorldObjectivesEntry> CurrentObjectives { get; set; } = new();
    [DataField]
    public List<WorldObjectivesEntry> CompletedObjectives { get; set; } = new();
    [DataField]
    public List<CodexEntry> CodexEntries { get; set; } = new();
    [DataField]
    [AutoNetworkedField]
    public Dictionary<string, CrewMetaRecord> CrewMetaRecords { get; set; } = new();
    [DataField]
    [AutoNetworkedField]
    public Dictionary<int, EntityUid> Stations { get; set; } = new();
    public bool TryGetRecord(string name, out CrewMetaRecord? record)
    {
        if (CrewMetaRecords.TryGetValue(name, out var currRecord))
        {
            record = currRecord;
            return true;
        }
        else
        {
            record = null;
            return false;
        }
    }
    public bool CreateRecord(string recordname, out CrewMetaRecord? record)
    {
        if (CrewMetaRecords.TryGetValue(recordname, out record)) return false;
        record = new CrewMetaRecord(recordname);
        CrewMetaRecords.Add(recordname, record);
        return true;
    }
    public bool TryEnsureRecord(string name, out CrewMetaRecord? record, EntityManager? entityManager = null)
    {
        if (TryGetRecord(name, out record)) return true;
        CreateRecord(name, out record);
        if (entityManager != null) entityManager.Dirty(Owner, this);
        return true;
    }
}


[DataDefinition]
[Serializable]
[Virtual]
public partial class CrewMetaRecord
{
    [DataField("_name")]
    public string Name = "Unnamed Crew Meta Record";
    [DataField]
    public DateTime LatestIDTime;
    [DataField]
    public ProtoId<NetworkLevelPrototype> Level = "NetworkLevel1";
    public CrewMetaRecord(string name)
    {
        Name = name;
    }
}
