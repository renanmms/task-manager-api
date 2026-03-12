using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.GetTaskByExpirationDate
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {
        [HttpGet("expires-date/{expiresAt}")]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByExpirationDate(DateOnly expiresAt)
        {
            var query = new GetTaskByExpirationDate.Query(expiresAt);
            var result = await mediator.Send(query);

            return Ok(result.Value);
        }
    }

    public static class GetTaskByExpirationDate
    {
        public class Query : IRequest<Result<List<TaskModel>>>
        {
            public DateOnly ExpiresAt { get; set; }
            public Query(DateOnly expiresAt)
            {
                ExpiresAt = expiresAt;
            }
        }

        internal sealed class Handler(TaskManagerDbContext context)
            : IRequestHandler<GetTaskByExpirationDate.Query, Result<List<TaskModel>>>
        {
            public async Task<Result<List<TaskModel>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tasks = context.Tasks.Where(t => DateOnly.FromDateTime(t.ExpiresAt) == request.ExpiresAt).ToList();

                return Result<List<TaskModel>>.Success(tasks);
            }
        }
    }
}