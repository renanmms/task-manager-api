using TaskManager.WebApi.Models;

namespace TaskManager.WebApi.Tests.Models
{
    public class TaskModelTests
    {
        private const string TITLE = "Task Manager API Project";
        [Fact]
        public void NewTask_WithTitle_ShouldSetTitleAndPendingStatus()
        {
            // Arrange
            var title = TITLE;

            // Act
            var task = new TaskModel(title);

            // Assert
            Assert.Equal(title, task.Title);
            Assert.Equal(TaskStatusEnum.Pending, task.Status);
        }

        [Fact]
        public void NewTask_WithAllParameters_ShouldSetProperties()
        {
            // Arrange
            var title = TITLE;
            var description = "Test Description";
            var expiresAt = DateTime.UtcNow.AddDays(1);
            var status = TaskStatusEnum.InProgress;

            // Act
            var task = new TaskModel(title, description, expiresAt, status);

            // Assert
            Assert.Equal(title, task.Title);
            Assert.Equal(description, task.Description);
            Assert.Equal(expiresAt, task.ExpiresAt);
            Assert.Equal(status, task.Status);
        }

        [Fact]
        public void Task_Start_ShouldSetStatusToInProgress()
        {
            // Arrange
            var task = new TaskModel(TITLE);

            // Act
            task.Start();

            // Assert
            Assert.Equal(TaskStatusEnum.InProgress, task.Status);
        }

        [Fact]
        public void Task_Finish_ShouldSetStatusToFinished()
        {
            // Arrange
            var task = new TaskModel(TITLE);

            // Act
            task.Finish();

            // Assert
            Assert.Equal(TaskStatusEnum.Finished, task.Status);
        }

        [Fact]
        public void Task_Update_ShouldUpdateAllFields()
        {
            // Arrange
            var task = new TaskModel("Old Title");

            var newTitle = "New Title";
            var newDescription = "New Description";
            var newExpiresAt = DateTime.UtcNow.AddDays(3);
            var newStatus = TaskStatusEnum.InProgress;

            // Act
            task.Update(newTitle, newDescription, newExpiresAt, newStatus);

            // Assert
            Assert.Equal(newTitle, task.Title);
            Assert.Equal(newDescription, task.Description);
            Assert.Equal(newExpiresAt, task.ExpiresAt);
            Assert.Equal(newStatus, task.Status);
        }
    }
}