using Microsoft.EntityFrameworkCore;
using TaskMgnrAPI.Entities;

namespace TaskMgnrAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItem { get; set; }
    }
}

