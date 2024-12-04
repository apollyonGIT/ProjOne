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
        UnitMoverJob unitMoverJob = new()
        { 
            deltaTime = SystemAPI.Time.DeltaTime, 
        };

        unitMoverJob.ScheduleParallel();
    }


    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
        {
            float3 moveDirection = math.normalize(unitMover.targetPosition - localTransform.Position);

            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(moveDirection, math.up()), deltaTime * unitMover.rotateSpeed);

            physicsVelocity.Linear = moveDirection * unitMover.moveSpeed;
            physicsVelocity.Angular = float3.zero;
        }
    }
}


