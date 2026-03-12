using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.FinishTask
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {

        [HttpPut("{id}/finish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Finish(int id)
        {
            var command = new FinishTask.Command(id);

            var result = await mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }
    }

    public static class FinishTask
    {

        public record Command(
            int Id
        ) : IRequest<Result>;

        internal sealed class Handler(TaskManagerDbContext context)
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var task = context.Tasks.SingleOrDefault(t => t.Id == request.Id);
                if(task == null)
                {
                    return Result.Failure(new Error("FinishTask.NotFound", "The task was not found"));
                }

                task.Finish();

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }
    }
}