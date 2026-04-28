using Content.Server.Chat.Systems;
using Content.Server.CrewRecords.Systems;
using Content.Server.EUI;
using Content.Shared.CrewAssignments.Systems;
using Content.Shared.Eui;
using Content.Shared.Follower;

namespace Content.Server.CrewAssignments.AdminUI;

public sealed class AdminWorldObjectivesEui : BaseEui
{
    private readonly CrewMetaRecordsSystem _crewMeta;
    private readonly ChatSystem _chat;
    public AdminWorldObjectivesEui()
    {
        IoCManager.InjectDependencies(this);
        _crewMeta = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<CrewMetaRecordsSystem>();
        _chat = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ChatSystem>();
    }

    public override void Opened()
    {
        StateDirty();
    }

    public override AdminWorldObjectivesEuiState GetNewState()
    {
        if (_crewMeta.MetaRecords == null) return new AdminWorldObjectivesEuiState(new List<WorldObjectivesEntry>(), new List<WorldObjectivesEntry>());
        List<WorldObjectivesEntry> entries = _crewMeta.MetaRecords.CurrentObjectives;
        List<WorldObjectivesEntry> completedEntries = _crewMeta.MetaRecords.CompletedObjectives;
        return new AdminWorldObjectivesEuiState(entries, completedEntries);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        switch (msg)
        {

            case AdminWorldObjectivesEuiMsg.DeleteCompleted deleteData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CompletedObjectives;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (entry.ID == deleteData.ID)
                        {
                            entries.Remove(entry);
                            break;
                        }
                    }
                    StateDirty();
                    break;
                }
            case AdminWorldObjectivesEuiMsg.Delete deleteData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CurrentObjectives;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (entry.ID == deleteData.ID)
                        {
                            entries.Remove(entry);
                            break;
                        }
                    }
                    StateDirty();
                    break;
                }
            case AdminWorldObjectivesEuiMsg.Complete revealData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CurrentObjectives;
                    var completedEntries = _crewMeta.MetaRecords.CompletedObjectives;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (entry.ID == revealData.ID)
                        {
                            entries.Remove(entry);
                            completedEntries.Add(entry);
                            entry.CompletedTime = DateTime.Now;
                            _chat.DispatchGlobalAnnouncement($"A historic achievement has been completed! {entry.Title}", "The Threshold");
                            break;
                        }
                    }
                    StateDirty();
                    break;
                }
            case AdminWorldObjectivesEuiMsg.Reveal revealData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CurrentObjectives;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (entry.ID == revealData.ID)
                        {
                            entry.Visible = true;
                            _chat.DispatchGlobalAnnouncement($"A new historic achievement has revealed itself! {entry.Title}");
                            break;
                        }
                    }
                    StateDirty();
                    break;
                }

            case AdminWorldObjectivesEuiMsg.CreateNew followData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CurrentObjectives;
                    WorldObjectivesEntry entry = new(_crewMeta.MetaRecords.NextObjectiveID);
                    _crewMeta.MetaRecords.NextObjectiveID++;
                    entries.Add(entry);
                    StateDirty();
                    break;
                }
            case AdminWorldObjectivesEuiMsg.SaveChanges saveData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CurrentObjectives;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (entry.ID == saveData.ID)
                        {
                            entry.Title = saveData.Title;
                            entry.Description = saveData.Description;
                            if (saveData.Reward == null || saveData.Reward == "")
                            {
                                entry.Reward = null;
                            }
                            else
                            {
                                entry.Reward = saveData.Reward;
                            }
                            if (saveData.CompletedDescription == null || saveData.CompletedDescription == "")
                            {
                                entry.CompletedDescription = null;
                            }
                            else
                            {
                                entry.CompletedDescription = saveData.CompletedDescription;
                            }
                            break;
                        }
                    }
                    StateDirty();
                    break;
                }
            case AdminWorldObjectivesEuiMsg.SaveChangesCompleted saveData:
                {
                    if (_crewMeta.MetaRecords == null) return;
                    var entries = _crewMeta.MetaRecords.CompletedObjectives;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        if (entry.ID == saveData.ID)
                        {
                            entry.Title = saveData.Title;
                            entry.Description = saveData.Description;
                            if (saveData.Reward == null || saveData.Reward == "")
                            {
                                entry.Reward = null;
                            }
                            else
                            {
                                entry.Reward = saveData.Reward;
                            }
                            if (saveData.CompletedDescription == null || saveData.CompletedDescription == "")
                            {
                                entry.CompletedDescription = null;
                            }
                            else
                            {
                                entry.CompletedDescription = saveData.CompletedDescription;
                            }
                            break;
                        }
                    }
                    StateDirty();
                    break;
                }
        }
    }
}

