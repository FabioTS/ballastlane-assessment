using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Dtos;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskDbContext _dbContext;

    public TaskController(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _dbContext.Tasks
            .OrderBy(task => task.Title)
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskItem>> GetById(Guid id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(item => item.Id == id);

        if (task is null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            ModelState.AddModelError(nameof(request.Title), "Title is required.");
        }

        if (request.UserId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(request.UserId), "UserId is required.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var newTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Status = request.Status,
            DueDate = request.DueDate,
            UserId = request.UserId
        };

        _dbContext.Tasks.Add(newTask);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            ModelState.AddModelError(nameof(request.Title), "Title is required.");
        }

        if (request.UserId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(request.UserId), "UserId is required.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var existingTask = await _dbContext.Tasks.FirstOrDefaultAsync(item => item.Id == id);

        if (existingTask is null)
        {
            return NotFound();
        }

        existingTask.Title = request.Title.Trim();
        existingTask.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        existingTask.Status = request.Status;
        existingTask.DueDate = request.DueDate;
        existingTask.UserId = request.UserId;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(item => item.Id == id);

        if (task is null)
        {
            return NotFound();
        }

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
