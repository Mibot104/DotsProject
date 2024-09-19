using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
[BurstCompile]
public partial class PlayerInputSystem : SystemBase
{
    private Input input;
    private Entity player;

    [BurstCompile]
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<PlayerMoveInput>();
        input = new Input();
    }

    [BurstCompile]
    protected override void OnStartRunning()
    {
        input.Enable();
        input.InputActions.Shoot.performed += OnShoot;
        player = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    [BurstCompile]
    private void OnShoot(InputAction.CallbackContext obj)
    {
        if (!SystemAPI.Exists(player)) return;
        
        SystemAPI.SetComponentEnabled<FireProjectileTag>(player, true);
    }
    
    [BurstCompile]
    protected override void OnStopRunning()
    {
        input.Disable();
        player = Entity.Null;
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector2 moveInput = input.InputActions.Movement.ReadValue<Vector2>();
        SystemAPI.SetSingleton(new PlayerMoveInput { Value = moveInput});
    }
}
