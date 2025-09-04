using Microsoft.EntityFrameworkCore;
using NoumanJobPortal.Models;
using NoumanJobPortal.Models.Users;

namespace NoumanJobPortal.Contracts
{
    public interface IApplicationDBContext
    {
        DbSet<Employer> Employers { get; }
        DbSet<JobSeeker> JobSeekers { get; }
        DbSet<Job> Jobs { get; }
        DbSet<Skill> Skills { get; }
        DbSet<JobApplication> JobApplications { get; }
        DbSet<ReviewStatus> ReviewStatuses { get; }
        DbSet<ApprovalStatus> ApprovalStatuses { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
