﻿using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, uintMover, physicsVelocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        {
            float3 targetPosition = MouseWorldPosition.instance.getPosition();
            float3 moveDirection = math.normalize(targetPosition - localTransform.ValueRO.Position);

            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, quaternion.LookRotation(moveDirection, math.up()), SystemAPI.Time.DeltaTime * uintMover.ValueRO.rotateSpeed);

            physicsVelocity.ValueRW.Linear = moveDirection * uintMover.ValueRO.moveSpeed;
            physicsVelocity.ValueRW.Angular = float3.zero;
            
            
        }

    }

}
