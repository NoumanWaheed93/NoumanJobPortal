
using Moq;
using Moq.EntityFrameworkCore;
using NoumanJobPortal.Contracts;
using NoumanJobPortal.Controllers;
using NoumanJobPortal.Models.Users;
using NoumanJobPortal.Models;
using NoumanJobPortal.Requests;
using Microsoft.AspNetCore.Mvc;

namespace NoumanJobPortal.Tests
{
    public class JobApplicationTests
    {
        private JobApplicationController _controller;
        private Mock<IApplicationDBContext> _dbContext;

        public JobApplicationTests()
        {
            _dbContext = new Mock<IApplicationDBContext>();
            _controller = new JobApplicationController(_dbContext.Object);
        }

        [Fact]
        public async Task ApplyToJob_ReturnsBadRequest_GivenANonExistingJob()
        {
            var request = new RequestApplyToJob
            {
                JobId = 999, // Assuming this ID does not exist in the test database
                JobSeekerId = 1 // Assuming this ID exists for the sake of this test
            };
            _dbContext.Setup(db => db.Jobs).ReturnsDbSet(new List<Job>()); // No jobs in the mock DB
            _dbContext.Setup(db => db.JobSeekers).ReturnsDbSet(new List<JobSeeker>
            {
                new JobSeeker { Id = 1, FirstName = "John", LastName = "Doe", User = new ApplicationUser() }
            });

            var result = await _controller.ApplyToJob(request);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ApplyToJob_ReturnsBadRequest_GivenANonExistingJobSeeker()
        {
            var request = new RequestApplyToJob
            {
                JobId = 1, // Assuming this ID exists for the sake of this test
                JobSeekerId = 999 // Assuming this ID does not exist in the test database
            };
            _dbContext.Setup(db => db.Jobs).ReturnsDbSet(new List<Job>
            {
                new Job { Id = 1,
                    Title = "Software Engineer",
                    Description = "Develop and maintain software applications.",
                    Employer = new Employer{CompanyName = "TestCompany", User = new ApplicationUser() },
                    ApprovalStatus = new ApprovalStatus{ Id = (int)EnumApprovalStatus.Approved, Name = "Approved" }
            }});
            _dbContext.Setup(db => db.JobSeekers).ReturnsDbSet(new List<JobSeeker>()); // No job seekers in the mock DB
            var result = await _controller.ApplyToJob(request);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task ApplyToJob_ReturnsBadRequest_GivenAlreadyApplied()
        {
            var request = new RequestApplyToJob
            {
                JobId = 1, // Assuming this ID exists for the sake of this test
                JobSeekerId = 1 // Assuming this ID exists for the sake of this test
            };

            Job job = new Job
            {
                Id = 1,
                Title = "Software Engineer",
                Description = "Develop and maintain software applications.",
                Employer = new Employer { CompanyName = "TestCompany", User = new ApplicationUser() },
                ApprovalStatus = new ApprovalStatus { Id = (int)EnumApprovalStatus.Approved, Name = "Approved" }
            };

            JobSeeker jobSeeker = new JobSeeker
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                User = new ApplicationUser()
            };

            _dbContext.Setup(db => db.Jobs).ReturnsDbSet(new List<Job>{job});
            _dbContext.Setup(db => db.JobSeekers).ReturnsDbSet(new List<JobSeeker>{ jobSeeker });
            _dbContext.Setup(db => db.JobApplications).ReturnsDbSet(new List<JobApplication>
            {
                new JobApplication
                {
                    Id = 1,
                    Job = job,
                    Applicant = jobSeeker,
                    ApplicationDate = DateTime.UtcNow,
                    ReviewStatus = new ReviewStatus { Id = (int)EnumReviewStatus.Pending, Name = "Pending" }
                }
            });
            var result = await _controller.ApplyToJob(request);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ApplyToJob_ReturnsOKResult_GivenValidData()
        {
            var request = new RequestApplyToJob
            {
                JobId = 1, // Assuming this ID exists for the sake of this test
                JobSeekerId = 1 // Assuming this ID exists for the sake of this test
            };
            Job job = new Job
            {
                Id = 1,
                Title = "Software Engineer",
                Description = "Develop and maintain software applications.",
                Employer = new Employer { CompanyName = "TestCompany", User = new ApplicationUser() },
                ApprovalStatus = new ApprovalStatus { Id = (int)EnumApprovalStatus.Approved, Name = "Approved" }
            };
            JobSeeker jobSeeker = new JobSeeker
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                User = new ApplicationUser()
            };
            _dbContext.Setup(db => db.Jobs).ReturnsDbSet(new List<Job> { job });
            _dbContext.Setup(db => db.JobSeekers).ReturnsDbSet(new List<JobSeeker> { jobSeeker });
            _dbContext.Setup(db => db.JobApplications).ReturnsDbSet(new List<JobApplication>()); // No existing applications
            _dbContext.Setup(db => db.ReviewStatuses).ReturnsDbSet(new List<ReviewStatus>
            {
                new ReviewStatus { Id = (int)EnumReviewStatus.Pending, Name = "Pending" }
            });
            _dbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);
            var result = await _controller.ApplyToJob(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
