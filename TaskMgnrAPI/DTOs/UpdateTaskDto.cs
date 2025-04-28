namespace TaskMgnrAPI.DTOs
{
    public class UpdateTaskDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime DueDate { get; set; }
        public string? Role { get; set; }
        public Guid? UserId { get; set; }
    }
}
