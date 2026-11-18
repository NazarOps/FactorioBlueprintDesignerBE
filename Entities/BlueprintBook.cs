namespace FactorioBlueprintDesigner.Entities
{
    public class BlueprintBook
    {
        public int Id { get; set; }

        // Foreign key
        public int AccountId { get; set; }

        // Navigation property
        public virtual Account Account { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Blueprint> Blueprints { get; set; } = new List<Blueprint>();
    }
}