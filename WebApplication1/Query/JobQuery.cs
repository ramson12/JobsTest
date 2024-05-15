namespace WebApplication1.Query
{
    public class JobQuery
    {
        public string? q { get; set; }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? LocationId { get; set; }
    public int? DepartmentId { get; set; }
    }
}
