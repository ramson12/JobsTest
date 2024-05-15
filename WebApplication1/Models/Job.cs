﻿namespace WebApplication1.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? Code { get; set; }  // Autogenerated
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
    }
}
