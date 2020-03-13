using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class HandlePlanetCollisionsSystem : JobComponentSystem {
    
    private struct TriggerJob : ITriggerEventsJob {

        public EntityCommandBuffer commandBuffer;

        public ComponentDataFromEntity<DamagableData> damagableEntities;
        [ReadOnly] public ComponentDataFromEntity<PlanetTag> planetEntities;  
        
        
        public void Execute(TriggerEvent triggerEvent) {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            bool planetIsEntityA = planetEntities.Exists(entityA);
            bool planetIsEntityB = planetEntities.Exists(entityB);

            bool damagableIsEntityA = damagableEntities.Exists(entityA);
            bool damagebleIsEntityB = damagableEntities.Exists(entityB);

            if (planetIsEntityA && damagebleIsEntityB) {
                commandBuffer.DestroyEntity(entityB);

            } else if (planetIsEntityB && damagableIsEntityA) {
                commandBuffer.DestroyEntity(entityA);
            }
        }
    }


    private BuildPhysicsWorld _buildPhysicsWorld;
    private StepPhysicsWorld _stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem _ecb;

    
    protected override void OnCreate() {
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        
        _ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        
        TriggerJob triggerJob = new TriggerJob {
            commandBuffer = _ecb.CreateCommandBuffer(),
            damagableEntities = GetComponentDataFromEntity<DamagableData>(),
            planetEntities = GetComponentDataFromEntity<PlanetTag>()
        };
        
        
        JobHandle triggerJobHandle = triggerJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
        _ecb.AddJobHandleForProducer(triggerJobHandle);

        return triggerJobHandle;
    }
}
