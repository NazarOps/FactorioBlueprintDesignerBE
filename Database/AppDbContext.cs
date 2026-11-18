using FactorioBlueprintDesigner.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactorioBlueprintDesigner.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blueprint> Blueprints { get; set; }
        public DbSet<BlueprintBook> BlueprintBooks { get; set; }
        public DbSet<BlueprintEntity> BlueprintEntities { get; set; }
        public DbSet<EntityDefinition> EntityDefinitions { get; set; }
    }
}