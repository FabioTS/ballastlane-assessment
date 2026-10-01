namespace TaskManagementSystem.Models;

public class TaskItem
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public TaskStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid UserId { get; set; }
}

public enum TaskStatus
{
    ToDo,
    InProgress,
    Blocked,
    Done
}
