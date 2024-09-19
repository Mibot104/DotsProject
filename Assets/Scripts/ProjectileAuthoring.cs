using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    public float ProjectileSpeed;
    public class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileMoveSpeed
            {
                Value = authoring.ProjectileSpeed
            });
        }
    }
}

public struct FireProjectileTag : IComponentData, IEnableableComponent {}
public struct IsDestroying : IComponentData {}

public struct ProjectileLifeTime : IComponentData
{
    public float Value;
}

