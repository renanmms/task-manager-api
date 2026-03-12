using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.CreateTask
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateTask.Command command)
        {
            var result = await mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtRoute("GetById", new { id = result.Value }, command);
        }
    }


    public static class CreateTask
    {
        public record Command(
            string Title,
            string Description,
            DateOnly ExpiresAt,
            TaskStatusEnum Status) : IRequest<Result<int>>
        {
            public TaskModel ToEntity()
            {
                return new TaskModel(
                    Title,
                    Description,
                    ExpiresAt.ToDateTime(TimeOnly.MinValue),
                    Status
                );
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(t => t.Title)
                    .NotEmpty()
                    .MinimumLength(15)
                    .MaximumLength(60);
            }
        }

        internal sealed class Handler(TaskManagerDbContext context, IValidator<CreateTask.Command> validator)
            : IRequestHandler<Command, Result<int>>
        {
            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result<int>.Failure(new Error("CreateTask.Validation", validationResult.ToString()));
                }

                var task = request.ToEntity();

                context.Tasks.Add(task);
                await context.SaveChangesAsync(cancellationToken);

                return Result<int>.Success(task.Id);
            }
        }
    }
}