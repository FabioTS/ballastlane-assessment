using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");

            entity.HasKey(task => task.Id);

            entity.Property(task => task.Title)
                .IsRequired();

            entity.Property(task => task.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(task => task.DueDate)
                .HasColumnType("TEXT");

            entity.Property(task => task.UserId)
                .IsRequired();
        });
    }
}
