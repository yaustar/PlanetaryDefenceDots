using Unity.Entities;

[GenerateAuthoringComponent]
public struct DamagableData : IComponentData {
    public int damage;
}