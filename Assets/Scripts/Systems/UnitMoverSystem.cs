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
            float3 moveDirection = unitMover.targetPosition - localTransform.Position;

            //停止判定
            if (math.lengthsq(moveDirection) < 2f)
            {
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;
                return;
            }
            moveDirection = math.normalize(moveDirection);

            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(moveDirection, math.up()), deltaTime * unitMover.rotateSpeed);
            
            physicsVelocity.Linear = moveDirection * unitMover.moveSpeed;
            physicsVelocity.Angular = float3.zero;
        }
    }
}


