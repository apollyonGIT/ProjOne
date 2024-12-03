using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, moveSpeed, physicsVelocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>())
        {
            float3 targetPosition = localTransform.ValueRO.Position + new float3(1,0,0);
            float3 moveDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);

            localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());

            physicsVelocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.value;
            physicsVelocity.ValueRW.Angular = float3.zero;
            
            
            
            //localTransform.ValueRW.Position = localTransform.ValueRO.Position + moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
        }

    }

}
