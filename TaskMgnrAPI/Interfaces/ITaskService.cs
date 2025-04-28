using TaskMgnrAPI.DTOs;
using TaskMgnrAPI.Entities;

namespace TaskMgnrAPI.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync(Guid userId, string role);
        Task<IEnumerable<User>> GetUserAsync(Guid userId, string role);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem> CreateTaskAsync(CreateTaskDto dto);
        Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(Guid id, Guid userId, string role);
    }
}
