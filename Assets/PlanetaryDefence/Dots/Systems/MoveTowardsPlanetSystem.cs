using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class MoveTowardsPlanetSystem : JobComponentSystem {

    	protected override JobHandle OnUpdate(JobHandle inputDeps) {

	        float dt = Time.DeltaTime;
	        
	        // WithName assigns the generated job class with a name making it easier to find when debugging 
    		Entities.WithName("MoveTowardsPlanet")
	            .ForEach((ref Translation translation, in MoveTowardsPlanetData moveTowardsPlanetData) => {
    			
		            // Target position is 0,0,0
	                float3 distVec = float3.zero - translation.Value;
	                distVec.z = 0f;
	                float3 direction = math.normalize(distVec);

	                float3 displacement = direction * moveTowardsPlanetData.speed * dt;

	                translation.Value = translation.Value + displacement;
	            }).Run();
    	    
    		return default;
    	}
}

