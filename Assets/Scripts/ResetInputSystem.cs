using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
[BurstCompile]
public partial struct ResetInputSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach (var (tag, entity) in SystemAPI.Query<FireProjectileTag>().WithEntityAccess())
        {
            ecb.SetComponentEnabled<FireProjectileTag>(entity, false);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
