
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using NoumanJobPortal.Contracts;
using NoumanJobPortal.Controllers;
using NoumanJobPortal.Models.Users;
using NoumanJobPortal.Requests;

namespace NoumanJobPortal.Tests
{
    public class JobSeekerTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private JobSeekerController _controller;
        private Mock<IApplicationDBContext> _dbContext;

        public JobSeekerTests()
        {
            var userManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            _userManagerMock = userManager;

            var dbContext = new Mock<IApplicationDBContext>();
            dbContext.Setup(db => db.JobSeekers).Returns(Mock.Of<DbSet<JobSeeker>>());
            _dbContext = dbContext;

            _controller = new JobSeekerController(userManager.Object, dbContext.Object);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_GivenANonExistingUserID()
        {
            // Act

            var request = new RequestCreateJobSeeker
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = "some-user"
            };

            var actionResult = await _controller.Register(request);

            // Assert

            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
      
        [Fact]
        public async Task Register_ReturnsOKResult_GivenValidData()
        {
            // Arrange

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { });

            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockData = new List<JobSeeker>
            {
            };

            _dbContext.Setup(js => js.JobSeekers)
                .ReturnsDbSet(mockData);

            // Act
            var request = new RequestCreateJobSeeker
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = "some-user"
            };

            var actionResult = await _controller.Register(request);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_GivenAPreexistingJobSeeker()
        {
            // Arrange
            ApplicationUser applicationUser = new ApplicationUser { Id = "some-user" };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(applicationUser);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockData = new List<JobSeeker>
            {
                new JobSeeker
                {
                    FirstName = "John",
                    LastName = "Doe",
                    User = applicationUser
                }
            };

            _dbContext.Setup(js => js.JobSeekers)
                .ReturnsDbSet(mockData);

            // Act
            var request = new RequestCreateJobSeeker
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = "some-user"
            };

            var actionResult = await _controller.Register(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}