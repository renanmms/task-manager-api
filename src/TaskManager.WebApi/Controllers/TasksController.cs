using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(TaskManagerDbContext context) : ControllerBase
    {
        private readonly TaskManagerDbContext _context = context;

        [HttpGet]
        public IActionResult Get()
        {
            var tasks = _context.Tasks.ToList();

            return Ok(tasks);
        }

        [HttpPost]
        public IActionResult Post(TaskModel model)
        {
            _context.Tasks.Add(model);
            _context.SaveChanges();

            return NoContent();
        }
    }
}