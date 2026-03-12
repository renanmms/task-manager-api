using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.DTOs;
using TaskManager.WebApi.Models;
using TaskManager.WebApi.Persistence;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(
        TaskManagerDbContext context,
        IValidator<NewTaskInputModel> createValidator,
        IValidator<EditTaskInputModel> editValidator) : ControllerBase
    {
        [HttpGet("expires-date/{expiresDate}")]
        [ProducesResponseType(typeof(List<TaskModel>), StatusCodes.Status200OK)]
        public IActionResult GetByExpiresDate(DateOnly expiresDate)
        {
            var tasks = context.Tasks.Where(t => DateOnly.FromDateTime(t.ExpiresAt) == expiresDate);

            return Ok(tasks);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, EditTaskInputModel model)
        {
            var task = context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            var validationResult = editValidator.Validate(model);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            task.Update(model.Title, model.Description, model.ExpiresDate, model.Status);
            context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/start")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Start(int id)
        {
            var task = context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            task.Start();
            context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/finish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Finish(int id)
        {
             var task = context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            task.Finish();
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var task = context.Tasks.SingleOrDefault(t => t.Id == id);
            if(task == null)
            {
                return NotFound();
            }

            context.Tasks.Remove(task);
            context.SaveChanges();

            return NoContent();
        }
    }
}