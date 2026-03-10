namespace TaskManager.WebApi.Models
{
    public class TaskModel(string title)
    {
        public string Title { get; set; } = title;
        public string? Description { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
    }

    public enum TaskStatusEnum
    {
        Pending,
        InProgress,
        Finished
    }
}