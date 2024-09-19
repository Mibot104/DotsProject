using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[BurstCompile]
public partial struct EnemyManagement : ISystem
{
    private float updateTime;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        updateTime -= deltaTime;
        if (updateTime <= 0)
        {
            updateTime = 0.1f;
            var playerTransform = SystemAPI.GetSingletonEntity<PlayerTag>();
            var playerTransformPosition = SystemAPI.GetComponent<LocalTransform>(playerTransform).Position;
            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
            foreach (var (direction, enemyTransform, followPlayerTime) in SystemAPI.Query<RefRW<EnemyDirection>, RefRO<LocalTransform>, RefRW<FollowPlayerTime>>())
            {
                
                if (followPlayerTime.ValueRW.Value > 0)
                {
                    followPlayerTime.ValueRW.Value -= updateTime;
                    direction.ValueRW.Value = math.normalizesafe(playerTransformPosition.xy - enemyTransform.ValueRO.Position.xy);
                }
                
                if (math.length(playerTransformPosition.xy - enemyTransform.ValueRO.Position.xy) < 0.5f)
                {
                    var destroyJob = new AddDestroy
                    {
                        Ecb = ecb
                    }.Schedule(state.Dependency);
                    state.Dependency = destroyJob;
                }
                
            }
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
        new EnemyJob
        {
            DeltaTime = deltaTime
        }.Schedule();
    }
}

[BurstCompile]
public partial struct AddDestroy : IJobEntity
{
    public EntityCommandBuffer Ecb;
    
    [BurstCompile]
    private void Execute(Entity entity)
    {
        Ecb.AddComponent<IsDestroying>(entity);
    }
}

[BurstCompile]
public partial struct EnemyJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(ref LocalTransform transform, in EnemyDirection direction, EnemyMovementSpeed movementSpeed)
    {
        transform.Position.xy += direction.Value * movementSpeed.Value * DeltaTime;
    }
}
