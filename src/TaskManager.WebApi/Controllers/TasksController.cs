using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.DTOs;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(TaskManagerDbContext context, IValidator<NewTaskInputModel> validator) : ControllerBase
    {
        private readonly TaskManagerDbContext _context = context;
        private readonly IValidator<NewTaskInputModel> _validator = validator;

        [HttpGet]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var tasks = _context.Tasks.ToList();

            return Ok(tasks);
        }

        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public IActionResult GetByStatus(TaskStatusEnum status)
        {
            var tasks = _context.Tasks.Where(t => t.Status == status);

            return Ok(tasks);
        }

        [HttpGet("expires-date/{expiresDate}")]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public IActionResult GetByExpiresDate(DateOnly expiresDate)
        {
            var tasks = _context.Tasks.Where(t => DateOnly.FromDateTime(t.ExpiresAt) == expiresDate);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(NewTaskInputModel model)
        {
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            var task = model.ToEntity();

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new {task.Id}, model);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, EditTaskInputModel model)
        {
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            task.Update(model.Title, model.Description, model.ExpiresDate, model.Status);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }
    }
}