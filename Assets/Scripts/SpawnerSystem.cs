using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                Entity entity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                float3 pos = new float3(spawner.ValueRO.SpawnPosition.x, spawner.ValueRO.SpawnPosition.y, 0);
                state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(pos));
                spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
            }
        }
    }
}
