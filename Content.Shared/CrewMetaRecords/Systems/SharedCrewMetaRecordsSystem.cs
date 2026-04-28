namespace Content.Shared.CrewMetaRecords;

public abstract partial class SharedCrewMetaRecordsSystem : EntitySystem
{
    private Queue<Action<CrewMetaRecordsComponent>> _pendingActions = new();
    public CrewMetaRecordsComponent? MetaRecords
    {
        get
        {
            var result = GetMetaRecordsComponent();
            return result;
        }
    }
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var metaRecords = GetMetaRecordsComponent();
        while (metaRecords != null && _pendingActions.TryDequeue(out var action))
            action.Invoke(metaRecords);
    }

    public CrewMetaRecordsComponent? GetMetaRecordsComponent()
    {
        var entityQuery = EntityQueryEnumerator<CrewMetaRecordsComponent>();
        if (!entityQuery.MoveNext(out _, out var metaRecords))
            return null;
        return metaRecords;
    }

    public void EnsureMetaRecordsAction(Action<CrewMetaRecordsComponent> action)
    {
        var metaRecords = GetMetaRecordsComponent();
        if (metaRecords == null)
            _pendingActions.Enqueue(action);
        else
            action.Invoke(metaRecords);
    }
}

