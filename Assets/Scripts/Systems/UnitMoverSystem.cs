using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>>())
        {
            float3 targetPosition = localTransform.ValueRO.Position + new float3(1,0,0);
            float3 moveDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);

            localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());
            localTransform.ValueRW.Position = localTransform.ValueRO.Position + moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
        }

    }

}
