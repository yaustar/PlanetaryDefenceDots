using Unity.Entities;
using UnityEngine;

public class MoveTowardsPlanetDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity {
    public float speed;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new MoveTowardsPlanetData() {speed = speed});
    }
}