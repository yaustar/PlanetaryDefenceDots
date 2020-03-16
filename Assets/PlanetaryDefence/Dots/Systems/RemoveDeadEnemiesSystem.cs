using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(HandlePlanetCollisionsSystem))]
public class RemoveDeadEnemiesSystem : SystemBase {
    
    protected override void OnUpdate() {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities.WithName("RemoveDeadEnemies")
            .ForEach((Entity entity, in HealthData healthData, in EnemyTag enemyTag) => {
                if (healthData.healthLeft <= 0) {
                    ecb.DestroyEntity(entity);
                }
            }).Run();
        
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
