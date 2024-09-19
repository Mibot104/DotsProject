using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct ProjectileMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltatime = SystemAPI.Time.DeltaTime;
        foreach (var (transform, movespeed) in SystemAPI.Query<RefRW<LocalTransform>, ProjectileMoveSpeed>())
        {
            transform.ValueRW.Position += transform.ValueRO.Up() * movespeed.Value * deltatime;
        }
    }
}
