using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Dtos;

public class UpdateTaskRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;

    public DateTime? DueDate { get; set; }

    [Required]
    public Guid UserId { get; set; }
}
