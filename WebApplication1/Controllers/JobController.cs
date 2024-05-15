using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Query;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/v1/jobs")]
    [ApiController]
    public class JobController : Controller
    {
        private readonly JobDbContext _context;

        public JobController(JobDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(JobRequest jobRequest)
        {
            if (jobRequest == null || jobRequest.LocationId == 0 || jobRequest.DepartmentId == 0)
            {
                return BadRequest();
            }

            // check if location exist
            var locationExist = await _context.Locations.FindAsync(jobRequest.LocationId);
            if (locationExist == null)
                return NotFound();

            // check if department exist
            var departmentExist = await _context.Departments.FindAsync(jobRequest.DepartmentId);
            if (departmentExist == null)
                return NotFound();

            var job = new Job {
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                LocationId = jobRequest.LocationId,
                DepartmentId = jobRequest.DepartmentId,
                ClosingDate = jobRequest.ClosingDate
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            // Get the ID of the newly created job
            var jobId = job.Id;

            // Construct the URL of the newly created job

            var jobUrl = $"http:localhost:5094/api/v1/jobs/{jobId}";

            return CreatedAtAction(nameof(CreateJob), jobUrl);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, JobRequest jobRequest)
        {
            if (id == 0)
                return BadRequest();


            // check if location exist
            var locationExist = await _context.Locations.FindAsync(jobRequest.LocationId);
            if (locationExist == null)
                return NotFound();

            // check if department exist
            var departmentExist = await _context.Departments.FindAsync(jobRequest.DepartmentId);
            if (departmentExist == null)
                return NotFound();

            var job = new Job
            {
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                LocationId = jobRequest.LocationId,
                DepartmentId = jobRequest.DepartmentId,
                ClosingDate = jobRequest.ClosingDate
            };

            var existingJob = await _context.Jobs.FindAsync(id);
            if (existingJob == null)
                return NotFound();

            // Update the existing job properties
            existingJob.Title = job.Title;
            existingJob.Description = job.Description;
            existingJob.LocationId = job.LocationId;
            existingJob.DepartmentId = job.DepartmentId;
            existingJob.ClosingDate = job.ClosingDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Location)
                .Include(j => j.Department)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
                return NotFound();

            return job;
        }

        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<Job>>> ListJobs(JobQuery query)
        {
            var jobsQuery = _context.Jobs.AsQueryable();

            if (!string.IsNullOrEmpty(query.q))
                jobsQuery = jobsQuery.Where(j => j.Title.Contains(query.q) || j.Description.Contains(query.q));

            if (query.LocationId.HasValue)
                jobsQuery = jobsQuery.Where(j => j.LocationId == query.LocationId.Value);

            if (query.DepartmentId.HasValue)
                jobsQuery = jobsQuery.Where(j => j.DepartmentId == query.DepartmentId.Value);

            var total = await jobsQuery.CountAsync();
            var jobs = await jobsQuery.Skip((query.PageNo - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            var jobList = jobs.Select(job => new JobList
            {
                Id = job.Id,
                Code = job.Code,
                Title = job.Title,
                Description = job.Description,
                LocationId = job.LocationId,
                DepartmentId = job.DepartmentId,
                ClosingDate = job.ClosingDate,
                PostedDate = job.PostedDate
            }).ToList();

            return Ok(new { total, data = jobList });
        }
    }
}
