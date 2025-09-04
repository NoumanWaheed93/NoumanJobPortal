using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoumanJobPortal.Models.Users;
using NoumanJobPortal.Database; // Assuming this is the namespace for your DbContext
using System.Threading.Tasks;
using NoumanJobPortal.Models;
using NoumanJobPortal.Requests;
using Microsoft.AspNetCore.Authorization;
using NoumanJobPortal.Contracts;

namespace NoumanJobPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JobSeekerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDBContext _dbContext;

        public JobSeekerController(UserManager<ApplicationUser> userManager, IApplicationDBContext dbContext) // Add dbContext to the constructor
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RequestCreateJobSeeker newJobSeeker)
        {
            // 1. Find the existing Identity user by email
            var identityUser = await _userManager.FindByIdAsync(newJobSeeker.UserId);
            if (identityUser == null)
                return BadRequest("User not found. Please register first using the default registration endpoint.");

            // 2. Assign JobSeeker role if not already assigned
            if (!await _userManager.IsInRoleAsync(identityUser, RolesStrings.JobSeeker))
            {
                var roleResult = await _userManager.AddToRoleAsync(identityUser, RolesStrings.JobSeeker);
                if (!roleResult.Succeeded)
                    return BadRequest(roleResult.Errors);
            }

            var exists = _dbContext.JobSeekers.Any(js => js.User.Id == identityUser.Id);
            if (exists)
                return BadRequest("JobSeeker profile already exists for this user.");

            // 3. Fetch and assign skills if provided
            Skill[] skillEntities = Array.Empty<Skill>();
            if (newJobSeeker.Skills != null && newJobSeeker.Skills.Length > 0)
            {
                skillEntities = _dbContext.Skills.Where(s => newJobSeeker.Skills.Contains(s.Id)).ToArray();
            }

            // 4. Create the JobSeeker entity and link to Identity user
            var jobSeeker = new JobSeeker
            {
                FirstName = newJobSeeker.FirstName,
                LastName = newJobSeeker.LastName,
                User = identityUser,
                skills = skillEntities
            };

            _dbContext.JobSeekers.Add(jobSeeker);
            await _dbContext.SaveChangesAsync();

            return Ok("JobSeeker profile created and role assigned successfully.");
        }
    }
}
