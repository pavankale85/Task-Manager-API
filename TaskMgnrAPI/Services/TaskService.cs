using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TaskMgnrAPI.Data;
using TaskMgnrAPI.DTOs;
using TaskMgnrAPI.Entities;
using TaskMgnrAPI.Interfaces;

namespace TaskMgnrAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync(Guid userId, string role)
        {
            return role == "Admin"
                ? await _context.TaskItem.ToListAsync()
                : await _context.TaskItem.Where(t => t.AssignedTo == userId).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserAsync(Guid userId, string role)
        {
            return role == "Admin"
                ? await _context.Users.ToListAsync()
                : await _context.Users.Where(t => t.Id == userId).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.TaskItem.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> CreateTaskAsync(CreateTaskDto dto)
        {
            var assignedTo = dto.Role == "Admin" 
                ? dto.AssignedTo
                : dto.UserId;

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                DueDate = dto.DueDate,
                AssignedTo = assignedTo
            };

            _context.TaskItem.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto dto)
        {
            var task = await _context.TaskItem.FindAsync(id);
            if (task == null) return false;
            if (dto.Role != "Admin" && task.AssignedTo != dto.UserId) return false;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(Guid id, Guid userId, string role)
        {
            var task = await _context.TaskItem.FindAsync(id);
            if (task == null) return false;
            if (role != "Admin" && task.AssignedTo != userId) return false;

            _context.TaskItem.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
