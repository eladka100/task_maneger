using Backend.Models;

namespace Backend.Services
{
    public class TaskService
    {
        public Dictionary<Guid, TaskItem> Tasks { get; } = new();

        public Guid addTask(string description)
        {
            Guid id = Guid.NewGuid();
            TaskItem task = new TaskItem { Id = id, Description = description, IsCompleted = false };
            Tasks.Add(id, task);
            return id;
        }
    }
}