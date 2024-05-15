using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;

namespace UnitTestJobs
{
    public class JobControllerTests
    {
        private readonly JobDbContext _context;
        private readonly JobController _controller;

        public JobControllerTests()
        {
            var options = new DbContextOptionsBuilder<JobDbContext>()
                .UseInMemoryDatabase(databaseName: "JobDatabase")
                .Options;

            _context = new JobDbContext(options);

            _controller = new JobController(_context);
        }

        [Fact]
        public async Task CreateJob_ReturnsSuccess()
        {
            // Arrange
            var job = new JobRequest
            {
                Title = "New Job",
                Description = "New Job Description",
                LocationId = 1,
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 08, 30)
            };

            // Act
            var result = await _controller.CreateJob(job);
        }


        [Fact]
        public async Task CreateJob_BadRequest_NullObject()
        {
            // Arrange
            var job = new JobRequest();

            // Act
            var result = await _controller.CreateJob(job);
        }

        [Fact]
        public async Task CreateJob_BadRequest_WithOutLocationId()
        {
            // Arrange
            var job = new JobRequest
            {
                Title = "New Job",
                Description = "New Job Description",
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 08, 30)
            };

            // Act
            var result = await _controller.CreateJob(job);
        }

        [Fact]
        public async Task CreateJob_BadRequest_WithOutDepartmentId()
        {
            // Arrange
            var job = new JobRequest
            {
                Title = "New Job",
                Description = "New Job Description",
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 08, 30)
            };

            // Act
            var result = await _controller.CreateJob(job);

        }


        [Fact]
        public async Task CreateJob_NotFound_LocationId()
        {
            // Arrange
            var job = new JobRequest
            {
                Title = "New Job",
                Description = "New Job Description",
                LocationId = 999,
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 08, 30)
            };

            // Act
            var result = await _controller.CreateJob(job);
          
        }

        [Fact]
        public async Task CreateJob_NotFound_DepartmentId()
        {
            // Arrange
            var job = new JobRequest
            {
                Title = "New Job",
                Description = "New Job Description",
                LocationId = 1,
                DepartmentId = 999,
                ClosingDate = new DateTime(2024, 08, 30)
            };

            // Act
            var result = await _controller.CreateJob(job);
            
        }




        [Fact]
        public async Task GetJob_WithValidId_ReturnsJob()
        {
            // Arrange
            var jobId = 1; // Assuming this is a valid job ID
            var job = new Job
            {
                Id = jobId,
                Title = "Software Developer",
                Description = "Job description here...",
                LocationId = 1,
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 08, 30), // Assuming this date
                Location = new Location { Id = 1, Title = "US Head Office", City = "Baltimore", State = "MD", Country = "United States", Zip = "21202" },
                Department = new Department { Id = 2, Title = "Software Development" }
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetJob(jobId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Job>>(result);
        }

        [Fact]
        public async Task GetJob_WithInvalidId_ReturnsNotFound()
        {
            // Arrange: Using an ID that does not exist in the database
            var invalidJobId = 9999;
            // Act
            var result = await _controller.GetJob(invalidJobId);
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task UpdateJob_ReturnsOkResult()
        {
            // Arrange
            int jobId = 1; // Assuming this is a valid job ID
            var updatedJob = new Job
            {
                Title = "Updated Job Title",
                Description = "Updated Job Description",
                LocationId = 1,
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 12, 31) // Updated closing date
            };

            // Add the updated job to the in-memory database
            _context.Jobs.Add(updatedJob);
            await _context.SaveChangesAsync();

            var updated = new JobRequest
            {
                Title = "Updated Job Title",
                Description = "Updated Job Description",
                LocationId = 1,
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 12, 31) // Updated closing date
            };


            // Act
            var result = await _controller.UpdateJob(jobId, updated);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateJob_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidJobId = 0; // Invalid job ID
            var updatedJob = new JobRequest
            {
                Title = "Updated Job Title",
                Description = "Updated Job Description",
                LocationId = 1,
                DepartmentId = 2,
                ClosingDate = new DateTime(2024, 12, 31) // Updated closing date
            };

            // Act
            var result = await _controller.UpdateJob(invalidJobId, updatedJob);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

    }
}