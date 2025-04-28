namespace TaskMgnrAPI.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } // "Pending", "InProgress", "Done"
        public DateTime DueDate { get; set; }
        public Guid AssignedTo { get; set; }
        //public User AssignedUser { get; set; }
    }
}
