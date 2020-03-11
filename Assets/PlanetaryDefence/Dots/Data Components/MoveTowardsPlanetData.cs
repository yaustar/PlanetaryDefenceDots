using Unity.Entities;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct  MoveTowardsPlanetData : IComponentData {
    public float2 velocity;
}