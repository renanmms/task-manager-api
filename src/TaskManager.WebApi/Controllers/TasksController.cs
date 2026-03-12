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
    }
}