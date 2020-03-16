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

        [ReadOnly] public ComponentDataFromEntity<DamageDealerData> damaeDealerEntities;
        [ReadOnly] public ComponentDataFromEntity<PlanetTag> planetEntities;  
        
        public ComponentDataFromEntity<HealthData> healthEntities;  
        
        public void Execute(TriggerEvent triggerEvent) {
            Entity entityA = triggerEvent.Entities.EntityA;
            Entity entityB = triggerEvent.Entities.EntityB;

            bool planetIsEntityA = planetEntities.Exists(entityA);
            bool planetIsEntityB = planetEntities.Exists(entityB);

            bool damagableIsEntityA = damaeDealerEntities.Exists(entityA);
            bool damagebleIsEntityB = damaeDealerEntities.Exists(entityB);

            if (planetIsEntityA && damagebleIsEntityB) {
                AddDamageToPlanet(entityA, entityB);

            } else if (planetIsEntityB && damagableIsEntityA) {
                AddDamageToPlanet(entityB, entityA);
            }
        }


        private void AddDamageToPlanet(Entity planetEntity, Entity damageDealerEntity) {
            DamageDealerData damageDealerData = damaeDealerEntities[damageDealerEntity];
            HealthData damageDealerHealthData = healthEntities[damageDealerEntity];

            // Destroy the thing that hit the planet
            damageDealerHealthData.healthLeft = 0;
            healthEntities[damageDealerEntity] = damageDealerHealthData;
            
            commandBuffer.AddComponent(planetEntity, new PlanetDamageData {damage = damageDealerData.damage} );
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
            damaeDealerEntities = GetComponentDataFromEntity<DamageDealerData>(true),
            planetEntities = GetComponentDataFromEntity<PlanetTag>(true),
            healthEntities = GetComponentDataFromEntity<HealthData>()
        };
        
        
        JobHandle triggerJobHandle = triggerJob.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
        _ecb.AddJobHandleForProducer(triggerJobHandle);

        return triggerJobHandle;
    }
}
