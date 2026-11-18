namespace FactorioBlueprintDesignerAPI.Contracts
{
    public class CreateBlueprintRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long Version { get; set; }
    }
}