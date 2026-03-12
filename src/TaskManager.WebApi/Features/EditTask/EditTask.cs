using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.EditTask
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, EditTask.Request request)
        {
            var command = new EditTask.Command(
                id,
                request.Title,
                request.Description,
                request.Status,
                request.ExpiresDate
            );

            var result = await mediator.Send(command);

            if (result.IsFailure)
            {
                return result.Error?.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(result.Error),
                    ErrorType.Validation => BadRequest(result.Error),
                    _ => BadRequest()
                };
            }

            return NoContent();
        }
    }

    public static class EditTask
    {
        public record Request(
            string Title,
            string Description,
            TaskStatusEnum Status,
            DateTime ExpiresDate
        );

        public record Command(
            int Id,
            string Title,
            string Description,
            TaskStatusEnum Status,
            DateTime ExpiresDate
        ) : IRequest<Result>;


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

        internal sealed class Handler(TaskManagerDbContext context, IValidator<EditTask.Command> validator)
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var task = context.Tasks.SingleOrDefault(t => t.Id == request.Id);

                if(task == null)
                {
                    return Result.Failure(new Error(ErrorType.NotFound, "The task was not found"));
                }

                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure(new Error(ErrorType.Validation, validationResult.ToString()));
                }

                task.Update(
                    request.Title,
                    request.Description,
                    request.ExpiresDate,
                    request.Status);

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }
    }
}