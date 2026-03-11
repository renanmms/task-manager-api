using Microsoft.EntityFrameworkCore;
using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Persistence
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) 
            : base(options)
        {
            
        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}