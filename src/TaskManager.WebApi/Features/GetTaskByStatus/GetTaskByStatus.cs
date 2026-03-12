using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.GetTaskByStatus
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStatus(TaskStatusEnum status)
        {
            var query = new GetTaskByStatus.Query(status);
            var result = await mediator.Send(query);

            return Ok(result.Value);
        }
    }

    public class GetTaskByStatus
    {


        public class Query : IRequest<Result<List<TaskModel>>>
        {
            public TaskStatusEnum Status { get; set; }
            public Query(TaskStatusEnum status)
            {
                Status = status;
            }
        }

        internal sealed class Handler(TaskManagerDbContext context)
            : IRequestHandler<GetTaskByStatus.Query, Result<List<TaskModel>>>
        {
            public async Task<Result<List<TaskModel>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tasks = context.Tasks.Where(t => t.Status == request.Status).ToList();

                return Result<List<TaskModel>>.Success(tasks);
            }
        }
    }
}