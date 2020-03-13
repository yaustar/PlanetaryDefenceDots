using Unity.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[GenerateAuthoringComponent]
public struct HealthData : IComponentData {
    public int healthLeft;
}
