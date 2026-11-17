using Carter;
using FactorioBlueprintDesigner.Database;
using FactorioBlueprintDesigner.Entities;
using FactorioBlueprintDesignerAPI.Contracts;
using FactorioBlueprintDesignerAPI.Shared;
using FluentValidation;
using MediatR;


namespace FactorioBlueprintDesignerAPI.Features.Blueprints
{
    public static class UpdateBlueprint
    {
        public class Command : IRequest<Result<int>>
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public bool ClearDescription { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Id).GreaterThan(0);
                RuleFor(c => c.Name).NotEmpty().When(c => c.Name != null);
                RuleFor(c => c.Description).Empty().When(c => c.ClearDescription == true);
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
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                
                if (!validationResult.IsValid)
                {
                    return Result.Failure<int>(new Error(
                        "UpdateBlueprint.Validation",
                        validationResult.ToString()));
                }

                var blueprint = await _dbContext.Blueprints.FindAsync([request.Id], cancellationToken);

                if (blueprint == null)
                {
                    return Result.Failure<int>(new Error(
                        "UpdateBlueprint.NotFound",
                        "Blueprint was not found."));
                }

                // Store original values
                var originalName = blueprint.Name;
                var originalDescription = blueprint.Description;

                // Update name if provided
                blueprint.Name = request.Name ?? blueprint.Name;

                // Update or clear description
                if (request.ClearDescription)
                {
                    blueprint.Description = null;
                }
                else if (request.Description is not null)
                {
                    blueprint.Description = request.Description;
                }

                // Check whether anything changed
                if (blueprint.Name != originalName || blueprint.Description != originalDescription)
                {
                    blueprint.Version++;

                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                return blueprint.Id;
            }

        }

        public class UpdateBlueprintEndpoint : ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder endpoint)
            {
                endpoint.MapPatch("api/blueprints/{id:int}", async (int id, UpdateBlueprintRequest request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new UpdateBlueprint.Command
                    {
                        Id = id,
                        Name = request.Name,
                        Description = request.Description,
                        ClearDescription = request.ClearDescription
                        
                    };

                    var result = await sender.Send(command, cancellationToken);

                    if (result.IsFailure) 
                    {
                        if(result.Error.Code == "UpdateBlueprint.NotFound")
                        {
                            return Results.NotFound(result.Error);
                        }

                        return Results.BadRequest(result.Error);
                    }

                    return Results.Ok(result.Value);
                });
            }
        }
    }
}
