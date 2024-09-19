using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spawner : IComponentData
{
    public Entity Prefab;
    public float2 SpawnPosition;
    public float NextSpawnTime;
    public float SpawnRate;
}
