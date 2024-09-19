using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float followTime;
    [SerializeField] private float lifeTime;
    
    public class EnemyAuthoringBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<EnemyTag>(enemyEntity);
            AddComponent(enemyEntity, new EnemyDirection
            {
                Value =  (Vector2) (authoring.player.transform.position - authoring.player.transform.position.normalized)
            });
            AddComponent(enemyEntity, new EnemyMovementSpeed
            {
                Value = authoring.movementSpeed
            });
            AddComponent(enemyEntity, new FollowPlayerTime
            {
                Value = authoring.followTime
            });
            AddComponent(enemyEntity, new ProjectileLifeTime
            {
                Value = authoring.lifeTime
            });
        }
    }
}

public struct EnemyTag: IComponentData {}

public struct EnemyDirection : IComponentData
{
    public float2 Value;
}

public struct EnemyMovementSpeed : IComponentData
{
    public float Value;
}

public struct FollowPlayerTime : IComponentData
{
    public float Value;
}
