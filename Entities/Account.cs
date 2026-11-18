namespace FactorioBlueprintDesigner.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Email { get; set; }

        public virtual ICollection<Blueprint> Blueprints { get; set; } = new List<Blueprint>();
        public virtual ICollection<BlueprintBook> BlueprintBooks { get; set; } = new List<BlueprintBook>();
    }
}