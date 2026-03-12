using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.StartTask
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {

        [HttpPut("{id}/start")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Start(int id)
        {
            var command = new StartTask.Command(id);

            var result = await mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }
    }

    public static class StartTask
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
                    return Result.Failure(new Error("StartTask.NotFound", "The task was not found"));
                }

                task.Start();

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }
    }
}