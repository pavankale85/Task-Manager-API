using System.Security.Claims;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskMgnrAPI.Data;
using TaskMgnrAPI.DTOs;
using TaskMgnrAPI.Entities;
using TaskMgnrAPI.Interfaces;

namespace TaskMgnrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        //public TaskController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetTasks()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (User.IsInRole("Admin"))
        //    {
        //        return Ok(await _context.TaskItem.ToListAsync());
        //    }
        //    return Ok(await _context.TaskItem.Where(t => t.AssignedTo == Guid.Parse(userId)).ToListAsync());
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        //{
        //    if (task.DueDate < DateTime.Now)
        //        return BadRequest("Due date cannot be in the past.");
        //    _context.TaskItem.Add(task);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem task)
        //{
        //    var existingTask = await _context.TaskItem.FindAsync(id);
        //    if (existingTask == null)
        //        return NotFound();
        //    existingTask.Title = task.Title;
        //    existingTask.Description = task.Description;
        //    existingTask.Status = task.Status;
        //    existingTask.DueDate = task.DueDate;
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}

        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string GetUserRole() => User.FindFirstValue(ClaimTypes.Role);

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] Guid userId, [FromQuery] string role)
        {
            var tasks = await _taskService.GetTasksAsync(userId, role);
            return Ok(tasks);
        }

        //[HttpGet]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUserList([FromQuery] Guid userId, [FromQuery] string role)
        {
            var tasks = await _taskService.GetUserAsync(userId, role);
            return Ok(tasks);
        }

        //public async Task<IActionResult> GetTasks()
        //{
        //    var tasks = await _taskService.GetTasksAsync(GetUserId(), GetUserRole());
        //    return Ok(tasks);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            try
            {
                var task = await _taskService.CreateTaskAsync(dto);
                //return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
                return Ok(new
                {
                    success = true,
                    message = "Record saved successfully",
                    data = task
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500,new
                {
                    success = false,
                    message = "Record saving failed",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var result = await _taskService.UpdateTaskAsync(id, dto);
                //return result ? NoContent() : Forbid();
                return Ok(new
                {
                    success = true,
                    message = "Record updated successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Record updating failed",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await _taskService.DeleteTaskAsync(id, GetUserId(), GetUserRole());
            return result ? NoContent() : Forbid();
        }
    }
}
