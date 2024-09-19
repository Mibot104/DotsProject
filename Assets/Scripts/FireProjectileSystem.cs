using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
[BurstCompile]
public partial struct FireProjectileSystem : ISystem
{       
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach (var (projectilePrefab, transform, lifeTime) in SystemAPI.Query<RefRO<ProjectilePrefab>, RefRO<LocalTransform>, RefRO<LifeTime>>().WithAll<FireProjectileTag>())
        { 
            var Projectile = ecb.Instantiate(projectilePrefab.ValueRO.Value);
            var ProjectilePrefabValues = state.EntityManager.GetComponentData<LocalTransform>(projectilePrefab.ValueRO.Value);
            var ProjectileTransform = LocalTransform.FromPositionRotationScale(transform.ValueRO.Position, transform.ValueRO.Rotation, ProjectilePrefabValues.Scale);
            ecb.SetComponent(Projectile, ProjectileTransform);
            ecb.AddComponent(Projectile, new ProjectileLifeTime { Value = lifeTime.ValueRO.Value });
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}