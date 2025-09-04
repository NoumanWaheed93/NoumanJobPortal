using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoumanJobPortal.Database;
using NoumanJobPortal.Models;
using NoumanJobPortal.Models.Users;
using NoumanJobPortal.Requests;

namespace NoumanJobPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _dbContext;

        public EmployerController(UserManager<ApplicationUser> userManager, ApplicationDBContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RequestCreateEmployer newEmployer)
        {
            // 1. Find the existing Identity user by email
            var identityUser = await _userManager.FindByIdAsync(newEmployer.UserId);
            if (identityUser == null)
                return BadRequest("User not found. Please register first using the default registration endpoint.");

            // 2. Assign Employer role if not already assigned
            if (!await _userManager.IsInRoleAsync(identityUser, "Employer"))
            {
                var roleResult = await _userManager.AddToRoleAsync(identityUser, "Employer");
                if (!roleResult.Succeeded)
                    return BadRequest(roleResult.Errors);
            }

            // 4. Create the Employer entity and link to Identity user
            var employer = new Employer
            {
                CompanyName = newEmployer.CompanyName,
                CompanyWebsite = newEmployer.CompanyWebsite,
                User = identityUser
            };

            _dbContext.Employers.Add(employer);
            await _dbContext.SaveChangesAsync();

            return Ok("JobSeeker profile created and role assigned successfully.");
        }
    }
}
