using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoumanJobPortal.Contracts;
using NoumanJobPortal.Database;
using NoumanJobPortal.Models;
using NoumanJobPortal.Requests;

namespace NoumanJobPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IApplicationDBContext _dbContext;

        public JobApplicationController(IApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = RolesStrings.JobSeeker)]
        [HttpPost]
        public async Task<IActionResult> ApplyToJob(RequestApplyToJob request)
        {
            var job = _dbContext.Jobs.FirstOrDefault(j => j.Id == request.JobId);
            if (job == null)
                return NotFound("Job not found.");
            
            var jobSeeker = _dbContext.JobSeekers.FirstOrDefault(js => js.Id == request.JobSeekerId);
            if (jobSeeker == null)
                return NotFound("Job seeker not found.");
            
            var alreadyApplied = _dbContext.JobApplications.Any(ja => ja.Job.Id == job.Id && ja.Applicant.Id == jobSeeker.Id);
            if(alreadyApplied)
                return BadRequest("You have already applied to this job.");

            var statusPending = _dbContext.ReviewStatuses.FirstOrDefault(rs => rs.Id == (int)EnumReviewStatus.Pending);

            var application = new JobApplication
            {
                Job = job,
                Applicant = jobSeeker,
                ApplicationDate = DateTime.UtcNow,
                ReviewStatus = statusPending,
            };
            _dbContext.JobApplications.Add(application);
            await _dbContext.SaveChangesAsync();
            return Ok("Application submitted successfully.");
        }
    }
}
