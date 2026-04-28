using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.CrewAssignments.Systems;

[Serializable, NetSerializable]
public sealed class AdminWorldObjectivesEuiState : EuiStateBase
{
    public List<WorldObjectivesEntry> Entries { get; }

    public List<WorldObjectivesEntry> CompletedEntries { get; }

    public AdminWorldObjectivesEuiState(List<WorldObjectivesEntry> entries, List<WorldObjectivesEntry> completedEntries)
    {
        Entries = entries;
        CompletedEntries = completedEntries;
    }
}
[DataDefinition, NetSerializable, Serializable]
[Virtual]
public partial class WorldObjectivesEntry
{
    [DataField("_id")]
    public int ID = 0;
    [DataField]
    public string Title { get; set; } = "";
    [DataField]
    public string Description { get; set; } = "";
    [DataField]
    public string? Reward { get; set; } = null;
    [DataField]
    public string? CompletedDescription { get; set; } = null;
    [DataField]
    public DateTime? CompletedTime { get; set; } = null;
    [DataField]
    public bool Visible = false;



    public WorldObjectivesEntry(int id)
    {
        ID = id;
    }
}

public static class AdminWorldObjectivesEuiMsg
{
    [Serializable, NetSerializable]
    public sealed class DeleteCompleted : EuiMessageBase
    {
        public DeleteCompleted(int iD)
        {
            ID = iD;
        }

        public int ID { get; }


    }
    [Serializable, NetSerializable]
    public sealed class Delete : EuiMessageBase
    {
        public Delete(int iD)
        {
            ID = iD;
        }

        public int ID { get; }


    }
    [Serializable, NetSerializable]
    public sealed class Complete : EuiMessageBase
    {
        public Complete(int iD)
        {
            ID = iD;
        }

        public int ID { get; }


    }
    [Serializable, NetSerializable]
    public sealed class Reveal : EuiMessageBase
    {
        public Reveal(int iD)
        {
            ID = iD;
        }

        public int ID { get; }


    }
    [Serializable, NetSerializable]
    public sealed class Close : EuiMessageBase
    {
    }

    [Serializable, NetSerializable]
    public sealed class CreateNew : EuiMessageBase
    {
    }
    [Serializable, NetSerializable]
    public sealed class SaveChangesCompleted : EuiMessageBase
    {
        public int ID { get; }
        public string Title { get; }
        public string Description { get; }
        public string? Reward { get; }
        public string? CompletedDescription { get; }

        public SaveChangesCompleted(int iD, string title, string description, string? reward, string? completedDescription)
        {
            ID = iD;
            Title = title;
            Description = description;
            Reward = reward;
            CompletedDescription = completedDescription;
        }
    }
    [Serializable, NetSerializable]
    public sealed class SaveChanges : EuiMessageBase
    {
        public int ID { get; }
        public string Title { get; }
        public string Description { get; }
        public string? Reward { get; }
        public string? CompletedDescription { get; }

        public SaveChanges(int iD, string title, string description, string? reward, string? completedDescription)
        {
            ID = iD;
            Title = title;
            Description = description;
            Reward = reward;
            CompletedDescription = completedDescription;
        }
    }
    [Serializable, NetSerializable]
    public sealed class Follow : EuiMessageBase
    {
        public NetEntity TargetWorldObjectives { get; }

        public Follow(NetEntity targetWorldObjectives)
        {
            TargetWorldObjectives = targetWorldObjectives;
        }
    }

    [Serializable, NetSerializable]
    public sealed class Send : EuiMessageBase
    {
        public NetEntity Target { get; }
        public string Title { get; }
        public string From { get; }
        public string Content { get; }
        public string StampState { get; }
        public Color StampColor { get; }
        public bool Locked { get; }

        public Send(NetEntity target, string title, string from, string content, string stamp, Color stampColor, bool locked)
        {
            Target = target;
            Title = title;
            From = from;
            Content = content;
            StampState = stamp;
            StampColor = stampColor;
            Locked = locked;
        }
    }
}
