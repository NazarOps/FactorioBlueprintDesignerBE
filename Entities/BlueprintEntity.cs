using FactorioBlueprintDesigner.Entities.Enum;

namespace FactorioBlueprintDesigner.Entities
{
    public class BlueprintEntity
    {
        public int Id { get; set; }

        // Foreign keys
        public int BlueprintId { get; set; }
        public int EntityDefinitionId { get; set; }

        // Navigation properties
        public virtual Blueprint Blueprint { get; set; } = null!;
        public virtual EntityDefinition EntityDefinition { get; set; } = null!;

        public decimal PositionX { get; set; }
        public decimal PositionY { get; set; }

        public Direction Direction { get; set; }
    }
}