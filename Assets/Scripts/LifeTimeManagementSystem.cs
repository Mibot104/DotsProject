using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
[BurstCompile]
public partial struct IsDestroyManagementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach (var (tag, entity) in SystemAPI.Query<IsDestroying>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct LifeTimeManagementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectileLifeTime>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        new LifeJob
        {
            Ecb = ecb,
            DeltaTime = deltaTime
        }.Schedule();
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

[BurstCompile]
public partial struct LifeJob : IJobEntity
{
    public EntityCommandBuffer Ecb;
    public float DeltaTime;
    
    [BurstCompile]
    public void Execute(Entity entity, ref ProjectileLifeTime lifeTime)
    {
        lifeTime.Value -= DeltaTime;
        if (lifeTime.Value <= 0)
        {
            Ecb.AddComponent<IsDestroying>(entity);
        }
    }
}