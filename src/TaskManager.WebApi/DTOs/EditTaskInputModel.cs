using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.DTOs
{
    public record EditTaskInputModel(
        string Title,
        string Description,
        TaskStatusEnum Status,
        DateTime ExpiresDate);
}