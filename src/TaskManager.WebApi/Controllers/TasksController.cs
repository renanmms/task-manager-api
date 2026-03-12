using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.DTOs;
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

        [HttpGet("status/{status}")]
        public IActionResult GetByStatus(TaskStatusEnum status)
        {
            var tasks = _context.Tasks.Where(t => t.Status == status);

            return Ok(tasks);
        }

        [HttpGet("expires-date/{expiresDate}")]
        public IActionResult GetByExpiresDate(DateOnly expiresDate)
        {
            var tasks = _context.Tasks.Where(t => DateOnly.FromDateTime(t.ExpiresAt) == expiresDate);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);

            return Ok(task);
        }

        [HttpPost]
        public IActionResult Post(TaskModel model)
        {
            _context.Tasks.Add(model);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new {model.Id}, model);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, EditTaskInputModel model)
        {
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);
            task?.Update(model.Title, model.Description, model.ExpiresDate, model.Status);

            _context.SaveChanges();

            return NoContent();
        }
    }
}