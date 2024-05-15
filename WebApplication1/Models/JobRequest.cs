namespace WebApplication1.Models
{
    public class JobRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}
