using Unity.Entities;

[GenerateAuthoringComponent]
public struct DamageDealerData : IComponentData {
    public int damage;
}