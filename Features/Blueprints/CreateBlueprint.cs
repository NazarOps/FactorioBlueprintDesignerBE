using Carter;
using FactorioBlueprintDesigner.Database;
using FactorioBlueprintDesigner.Entities;
using FactorioBlueprintDesignerAPI.Contracts;
using FactorioBlueprintDesignerAPI.Shared;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;

namespace FactorioBlueprintDesignerAPI.Features.Blueprints
{
    public static class CreateBlueprint
    {
        public class Command : IRequest<Result<int>>
        {
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public long Version { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
           public Validator()
            {
                RuleFor(c => c.Name).NotEmpty();
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Result<int>>
        {
            private readonly AppDbContext _dbContext;
            private readonly IValidator<Command> _validator;

            public Handler(AppDbContext dbContext, IValidator<Command> validator)
            {
                _dbContext = dbContext;
                _validator = validator;
            }

            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);

                if (!validationResult.IsValid)
                {
                    return Result.Failure<int>(new Error(
                        "CreateBlueprint.Validation",
                        validationResult.ToString()));
                }
                
                var blueprint = new Blueprint
                {
                    Name = request.Name,
                    Description = request.Description,
                    Version = request.Version,
                    CreatedAt = DateTime.UtcNow,
                    AccountId = 1
                };

                _dbContext.Add(blueprint);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return blueprint.Id;
            }
        }

        public class CreateBlueprintEndpoint : ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder endpoint)
            {
                endpoint.MapPost("api/blueprints", async (CreateBlueprintRequest request, ISender sender) =>
                {
                    var command = new CreateBlueprint.Command
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Version = request.Version
                    };

                    var result = await sender.Send(command);

                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);
                });
            }
        }
    }
}
