using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.DTOs
{
    public record NewTaskInputModel(string Title, string Description, DateOnly ExpiresAt, TaskStatusEnum Status)
    {
        public TaskModel ToEntity()
        {
            return new TaskModel(
                Title,
                Description,
                ExpiresAt.ToDateTime(TimeOnly.MinValue),
                Status
            );
        }   
    }
}