namespace TaskMgnrAPI.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime DueDate { get; set; }
        public Guid AssignedTo { get; set; }
        public string? Role { get; set; }
        public Guid UserId { get; set; }
    }
}
