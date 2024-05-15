using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class JobDbContext : DbContext
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options)
        {

        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Department> Departments { get; set; }

        public override int SaveChanges()
        {
            GenerateJobCodes();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            GenerateJobCodes();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void GenerateJobCodes()
        {
            var entries = ChangeTracker.Entries<Job>()
                                       .Where(e => e.State == EntityState.Added && string.IsNullOrEmpty(e.Entity.Code));

            foreach (var entry in entries)
            {
                entry.Entity.Code = GenerateJobCode();
            }
        }

        private string GenerateJobCode()
        {
            var latestJob = Jobs.OrderByDescending(j => j.Id).FirstOrDefault();
            int nextId = latestJob != null ? latestJob.Id + 1 : 1;
            return $"JOB-{nextId:D4}";  // Format as "JOB-0001", "JOB-0002", etc.
        }

    }
}
