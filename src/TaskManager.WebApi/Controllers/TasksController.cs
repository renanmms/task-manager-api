using System;
using Microsoft.AspNetCore.Mvc;
using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var task = new TaskModel("My First Task");
            return Ok(task);
        }
    }
}