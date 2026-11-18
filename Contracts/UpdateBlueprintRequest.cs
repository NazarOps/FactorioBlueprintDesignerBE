namespace FactorioBlueprintDesignerAPI.Contracts
{
    public class UpdateBlueprintRequest
    {
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool ClearDescription { get; set; }
    }
}