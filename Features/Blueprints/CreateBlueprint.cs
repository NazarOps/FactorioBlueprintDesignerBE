using FactorioBlueprintDesigner.Database;
using FactorioBlueprintDesigner.Entities;
using MediatR;

namespace FactorioBlueprintDesigner.Features.Blueprints
{
    public static class CreateBlueprint
    {
        public class Command : IRequest<Guid>
        {
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public long Version { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, Guid>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var blueprint = new Blueprint
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Version = request.Version,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Add(blueprint);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return blueprint.Id;
            }
        }
    }
}
