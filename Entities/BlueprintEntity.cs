using FactorioBlueprintDesigner.Entities.Enum;

namespace FactorioBlueprintDesigner.Entities
{
    public class BlueprintEntity
    {
        public Guid Id { get; set; }

        // Foreign keys
        public Guid BlueprintId { get; set; }
        public Guid EntityDefinitionId { get; set; }

        // Navigation properties
        public virtual Blueprint Blueprint { get; set; } = null!;
        public virtual EntityDefinition EntityDefinition { get; set; } = null!;

        public decimal PositionX { get; set; }
        public decimal PositionY { get; set; }

        public Direction Direction { get; set; }
    }
}