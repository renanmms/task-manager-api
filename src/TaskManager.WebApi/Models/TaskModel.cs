namespace TaskManager.WebApi.Models
{
    public class TaskModel
    {

        public TaskModel(string title)
        {
            Title = title;
        }

        public TaskModel(string title, string? description, DateTime expiresAt, TaskStatusEnum status)
        {
            Title = title;
            Description = description;
            ExpiresAt = expiresAt;
            Status = status;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime ExpiresAt { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;

        public void Update(string title, string description, DateTime expiresAt, TaskStatusEnum status)
        {
            Title = title;
            Description = description;
            ExpiresAt = expiresAt;
            Status = status;
        }
    }

    public enum TaskStatusEnum
    {
        Pending,
        InProgress,
        Finished
    }
}