
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using NoumanJobPortal.Contracts;
using NoumanJobPortal.Controllers;
using NoumanJobPortal.Models;
using NoumanJobPortal.Models.Users;
using NoumanJobPortal.Requests;

namespace NoumanJobPortal.Tests
{
    public class JobTests
    {
        private JobController _controller;
        private Mock<IApplicationDBContext> _dbContext;

        public JobTests()
        {
            var dbContext = new Mock<IApplicationDBContext>();
            dbContext.Setup(db => db.JobSeekers).Returns(Mock.Of<DbSet<JobSeeker>>());
            _dbContext = dbContext;

            _controller = new JobController(dbContext.Object);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_GivenANonExistingEmployer()
        {
            var request = new RequestCreateJob
            {
                Title = "Software Engineer",
                Description = "Develop and maintain software applications.",
                SkillIds = null, // Assuming no skills are required for this test
                EmployerId = 999 // Assuming this ID does not exist in the test database
            };
        
            _dbContext.Setup(db => db.Employers).ReturnsDbSet(new List<Employer>()); // No employers in the mock DB

            var result = await _controller.PostJob(request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsOKResult_GivenValidData()
        {
            // Arrange
            var mockEmployers = new List<Employer>
            {
                new Employer { Id = 1, CompanyName = "Tech Corp", User = new ApplicationUser() }
            };
            _dbContext.Setup(db => db.Employers).ReturnsDbSet(mockEmployers);
            _dbContext.Setup(db => db.ApprovalStatuses).ReturnsDbSet(new List<ApprovalStatus>
            {
                new ApprovalStatus { Id = (int)EnumApprovalStatus.Pending, Name = "Pending" }
            });
            _dbContext.Setup(db => db.Jobs).ReturnsDbSet(new List<Job>());
            var request = new RequestCreateJob
            {
                Title = "Software Engineer",
                Description = "Develop and maintain software applications.",
                SkillIds = null, // Assuming no skills are required for this test
                EmployerId = 1 // Existing employer ID
            };
            // Act
            var result = await _controller.PostJob(request);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
