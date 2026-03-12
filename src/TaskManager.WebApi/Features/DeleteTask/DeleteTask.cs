using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.DeleteTask
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteTask.Command(id);
            var result = await mediator.Send(command);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }
    }

    public static class DeleteTask
    {
        public record Command(int Id) : IRequest<Result>;

        internal sealed class Handler(TaskManagerDbContext context) 
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var task = await context.Tasks.SingleOrDefaultAsync(t => t.Id == request.Id);
                if(task == null)
                {
                    return Result.Failure(new Error("DeleteTask", "The task was not found"));
                }

                context.Tasks.Remove(task);
                await context.SaveChangesAsync();

                return Result.Success();
            }
        }
    }
}