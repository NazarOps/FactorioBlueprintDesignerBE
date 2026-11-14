namespace FactorioBlueprintDesigner.Entities
{
    public class EntityDefinition
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Icon { get; set; } = null!;

        public virtual ICollection<BlueprintEntity> Entities { get; set; } = new List<BlueprintEntity>();
    }
}