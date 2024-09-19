using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float SpawnRate;

    [BurstCompile]
    class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        [BurstCompile]
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Spawner
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                SpawnPosition = float2.zero,
                NextSpawnTime = authoring.SpawnRate,
                SpawnRate = authoring.SpawnRate
            });
        }
    }
}
