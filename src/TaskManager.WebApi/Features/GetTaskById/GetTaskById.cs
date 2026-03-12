using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;
using static TaskManager.WebApi.Features.GetTaskById.GetTaskById;

namespace TaskManager.WebApi.Features.GetTaskById
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {

        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(TaskModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTaskById.Query(id);
            var result = await mediator.Send(query);
            if(result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
    }

    public static class GetTaskById
    {
        public class Query : IRequest<Result<TaskModel>>
        {
            public int Id { get; set; }
            public Query(int id)
            {
                Id = id;
            }
        }
    }

    internal sealed class Handler(TaskManagerDbContext context) 
        : IRequestHandler<GetTaskById.Query, Result<TaskModel>>
    {
        public async Task<Result<TaskModel>> Handle(Query request, CancellationToken cancellationToken)
        {
            var task = await context.Tasks.SingleOrDefaultAsync(t => t.Id == request.Id);

            if(task == null)
            {
                return Result<TaskModel>.Failure(new Error("GetTaskById.NotFound", "The task was not found"));
            }

            return Result<TaskModel>.Success(task);
        }
    }
}