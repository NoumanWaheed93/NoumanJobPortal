using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoumanJobPortal.Contracts;
using NoumanJobPortal.Database;
using NoumanJobPortal.Models;
using NoumanJobPortal.Requests;

namespace NoumanJobPortal.Controllers
{
    [Authorize(Roles = RolesStrings.Employer)]
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IApplicationDBContext _dbContext;
        public JobController(IApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostJob(RequestCreateJob newJob)
        {
            var employer = _dbContext.Employers.FirstOrDefault(e => e.Id == newJob.EmployerId);
            if (employer == null)
                return BadRequest("Employer not found.");
            var job = new Job
            {
                Title = newJob.Title,
                Description = newJob.Description,
                Employer = employer,
                ApprovalStatus = _dbContext.ApprovalStatuses.FirstOrDefault(s => s.Id == (int)EnumApprovalStatus.Pending)!
            };
            _dbContext.Jobs.Add(job);
            await _dbContext.SaveChangesAsync();
            
            return Ok("Job posted successfully.");
        }
    }
}
