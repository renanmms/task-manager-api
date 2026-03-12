namespace TaskManager.WebApi.Models
{
    public class TaskModel(string title)
    {
        public int Id { get; set; }
        public string Title { get; set; } = title;
        public string? Description { get; set; }
        public DateTime ExpiresAt { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
    }

    public enum TaskStatusEnum
    {
        Pending,
        InProgress,
        Finished
    }
}