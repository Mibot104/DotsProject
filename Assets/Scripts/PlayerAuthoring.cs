using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public GameObject ProjectilePrefab;
    public float ProjectileLifeTime;
    
    class PlayerAuthoringBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<PlayerTag>(playerEntity);
            AddComponent<PlayerMoveInput>(playerEntity);
            AddComponent(playerEntity, new PlayerMoveSpeed
            {
                Value = authoring.MoveSpeed
            });
            
            AddComponent<FireProjectileTag>(playerEntity);
            SetComponentEnabled<FireProjectileTag>(playerEntity, false);
            
            AddComponent(playerEntity, new ProjectilePrefab
            {
                Value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
            });
            AddComponent(playerEntity, new LifeTime
            {
                Value = authoring.ProjectileLifeTime
            });
        }
    }
}

public struct PlayerMoveInput : IComponentData
{
    public float2 Value;
}

public struct PlayerMoveSpeed: IComponentData
{
    public float Value;
}

public struct PlayerTag: IComponentData {}

public struct ProjectilePrefab : IComponentData
{
    public Entity Value;
}

public struct ProjectileMoveSpeed : IComponentData
{
    public float Value;
}

public struct LifeTime : IComponentData
{
    public float Value;
};