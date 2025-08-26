using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoumanJobPortal.Models.Users;
using NoumanJobPortal.Database; // Assuming this is the namespace for your DbContext
using System.Threading.Tasks;
using NoumanJobPortal.Models;
using NoumanJobPortal.Requests;

namespace NoumanJobPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobSeekerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _dbContext; // Add this line

        public JobSeekerController(UserManager<ApplicationUser> userManager, ApplicationDBContext dbContext) // Add dbContext to the constructor
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateJobSeeker newJobSeeker)
        {
            // 1. Create the Identity user
            var identityUser = new ApplicationUser
            {
                UserName = newJobSeeker.UserName,
                Email = newJobSeeker.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(identityUser, newJobSeeker.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 2. Assign JobSeeker role
            await _userManager.AddToRoleAsync(identityUser, "JobSeeker");

            // 2a. Fetch and assign skills if provided
            Skill[] skillEntities = Array.Empty<Skill>();
            if (newJobSeeker.Skills != null && newJobSeeker.Skills.Length > 0)
            {
                skillEntities = _dbContext.Skills.Where(s => newJobSeeker.Skills.Contains(s.Id)).ToArray();
            }

            // 3. Create the JobSeeker entity and link to Identity user
            var jobSeeker = new JobSeeker
            {
                FirstName = newJobSeeker.FirstName,
                LastName = newJobSeeker.LastName,
                User = identityUser,
                skills = skillEntities
            };

            // Save JobSeeker to the database (use your DbContext)
            _dbContext.JobSeekers.Add(jobSeeker);
            await _dbContext.SaveChangesAsync();

            return Ok("JobSeeker account created successfully.");
        }
    }
}
