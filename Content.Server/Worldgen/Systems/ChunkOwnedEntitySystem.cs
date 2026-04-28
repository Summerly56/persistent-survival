using Content.Server.Worldgen.Components;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.Worldgen.Systems;

public sealed class ChunkOwnedEntitySystem : BaseWorldSystem
{
    private Dictionary<EntityUid, Entity<WorldChunkComponent>> _cachedEntityToChunkMap = new();
    private Dictionary<EntityUid, HashSet<Entity<ChunkOwnedEntityComponent>>> _cachedChunkToEntitiesMap = new();

    private float _updateTimer = 0;
    private const float UpdateTime = 1.0f;

    /// <summary>
    /// Not really a queue - we only need to process all entities in this update list
    /// while keeping it hashed so an entity doesnt get updated more than once.
    /// </summary>
    private HashSet<Entity<ChunkOwnedEntityComponent>> _updateQueue = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ChunkOwnedEntityComponent, ComponentStartup>(OnOwnedEntityStartup);
        SubscribeLocalEvent<ChunkOwnedEntityComponent, ComponentShutdown>(OnOwnedEntityShutdown);
        SubscribeLocalEvent<ChunkOwnedEntityComponent, MoveEvent>(OnOwnedEntityMoved);
        SubscribeLocalEvent<WorldChunkComponent, WorldChunkUnloadedEvent>(OnChunkUnloaded);
    }

    private void OnOwnedEntityStartup(Entity<ChunkOwnedEntityComponent> ent, ref ComponentStartup args)
    {
        _updateQueue.Add(ent);
    }

    private void OnOwnedEntityShutdown(Entity<ChunkOwnedEntityComponent> ent, ref ComponentShutdown args)
    {
        _updateQueue.Add(ent);
    }

    private void OnOwnedEntityMoved(Entity<ChunkOwnedEntityComponent> ent, ref MoveEvent args)
    {
        _updateQueue.Add(ent);
    }

    private void OnChunkUnloaded(Entity<WorldChunkComponent> ent, ref WorldChunkUnloadedEvent args)
    {
        if (_cachedChunkToEntitiesMap.TryGetValue(ent, out var ownedEntities))
            foreach (var ownedEnt in ownedEntities)
                _updateQueue.Add(ownedEnt);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        _updateTimer += frameTime;
        if (_updateTimer < UpdateTime)
            return;
        _updateTimer -= UpdateTime;

        if (_updateQueue.Count == 0)
            return;

        foreach (var toUpdate in _updateQueue)
        {
            UpdateOwnedEntity(toUpdate);
        }
        _updateQueue.Clear();
    }

    private void UpdateOwnedEntity(Entity<ChunkOwnedEntityComponent> ent)
    {
        if (TerminatingOrDeleted(ent))
        {
            if (_cachedEntityToChunkMap.TryGetValue(ent, out var cur))
            {
                _cachedChunkToEntitiesMap[cur].Remove(ent);
                _cachedEntityToChunkMap.Remove(ent);
            }
            return;
        }

        var chunkUid = GetOrCreateChunkAt(ent);
        if (
            !TryComp<WorldChunkComponent>(chunkUid, out var chunkData)
            || chunkData == null
            || !TryComp<LoadedChunkComponent>(chunkUid, out var loadedChunk)
            || loadedChunk == null
        )
        {
            // Despawn
            QueueDel(ent);
            return;
        }

        // Remove from previous chunk's cache if applicable.
        if (_cachedEntityToChunkMap.TryGetValue(ent, out var currentChunk))
        {
            if (currentChunk == chunkUid)
                return; // Same chunk - ignore update.
            _cachedChunkToEntitiesMap[currentChunk].Remove(ent);
        }

        // Cache entity's current chunk
        if (!_cachedChunkToEntitiesMap.ContainsKey(chunkUid.Value))
            _cachedChunkToEntitiesMap.Add(chunkUid.Value, new());
        _cachedChunkToEntitiesMap[chunkUid.Value].Add(ent);
        _cachedEntityToChunkMap[ent] = new(chunkUid.Value, chunkData);
    }

    public void GenerateEntity(EntProtoId spawnProto, EntityCoordinates entityCoordinates)
    {
        var spawned = SpawnAtPosition(spawnProto, entityCoordinates);
        BeginTrackingEntity(spawned);
    }

    public void BeginTrackingEntity(EntityUid uid)
    {
        EnsureComp<ChunkOwnedEntityComponent>(uid);
    }
}
