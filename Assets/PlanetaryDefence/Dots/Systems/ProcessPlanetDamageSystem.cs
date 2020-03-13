using Unity.Collections;
using Unity.Entities;


[UpdateAfter(typeof(HandlePlanetCollisionsSystem))]
public class ProcessPlanetDamageSystem : SystemBase {
    protected override void OnUpdate() {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities.WithName("ProcessPlanetDamage")
            .ForEach((Entity entity, ref HealthData healthData, in PlanetTag planetTag, in PlanetDamageData planetDamageData) => {
                healthData.healthLeft -= planetDamageData.damage; 
                ecb.RemoveComponent<PlanetDamageData>(entity);
            }).Run();
        
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
