using Unity.Entities;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveTowardsPlanetDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity {
    public float2 velocity;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new MoveTowardsPlanetData() {velocity = velocity});
    }
}