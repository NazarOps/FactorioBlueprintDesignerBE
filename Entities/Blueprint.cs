namespace FactorioBlueprintDesigner.Entities
{
    public class Blueprint
    {
        public int Id { get; set; }

        // Foreign keys
        public int AccountId { get; set; }
        public int? BlueprintBookId { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; } = null!;
        public virtual BlueprintBook? BlueprintBook { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long Version { get; set; }

        public virtual ICollection<BlueprintEntity> Entities { get; set; } = new List<BlueprintEntity>();
    }
}