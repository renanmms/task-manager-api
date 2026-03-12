using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;
using TaskManager.WebApi.Shared;

namespace TaskManager.WebApi.Features.GetAllTasks
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class TasksController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllTasks.Query();
            var result = await mediator.Send(query);

            return Ok(result.Value);
        }

    }

    public static class GetAllTasks
    {
        public class Query : IRequest<Result<List<TaskModel>>>
        {

        }

        internal sealed class Handler(TaskManagerDbContext context) 
            : IRequestHandler<GetAllTasks.Query, Result<List<TaskModel>>>
        {
            public async Task<Result<List<TaskModel>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var tasks = await context.Tasks.ToListAsync();

                return Result<List<TaskModel>>.Success(tasks);
            }
        }
    }
}