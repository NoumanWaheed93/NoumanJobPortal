using NoumanJobPortal.Controllers;

namespace JobPortalUnitTestingProject
{
    public class JobSeekerTests
    {
        [Fact]
        public void Test1()
        {
            //Assemble

            var userManagerFake = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var controller = new JobSeekerController();

            //Act
            //Assert
        }
    }
}