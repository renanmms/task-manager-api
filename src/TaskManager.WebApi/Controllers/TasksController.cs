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