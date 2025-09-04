using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoumanJobPortal.Contracts;
using NoumanJobPortal.Models;
using NoumanJobPortal.Models.Users;

namespace NoumanJobPortal.Database
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>, IApplicationDBContext
    {
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<ReviewStatus> ReviewStatuses { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }


        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");

            builder.Entity<ReviewStatus>()
                .HasData(
                    new ReviewStatus { Id = (int)EnumReviewStatus.Pending, Name = EnumReviewStatus.Pending.ToString() },
                    new ReviewStatus { Id = (int)EnumReviewStatus.Reviewed, Name = EnumReviewStatus.Reviewed.ToString() },
                    new ReviewStatus { Id = (int)EnumReviewStatus.Accepted, Name = EnumReviewStatus.Accepted.ToString() },
                    new ReviewStatus { Id = (int)EnumReviewStatus.Rejected, Name = EnumReviewStatus.Rejected.ToString() }
                );

            builder.Entity<ApprovalStatus>()
                .HasData(
                    new ApprovalStatus { Id = (int)EnumApprovalStatus.Pending, Name = EnumApprovalStatus.Pending.ToString() },
                    new ApprovalStatus { Id = (int)EnumApprovalStatus.Approved, Name = EnumApprovalStatus.Approved.ToString() },
                    new ApprovalStatus { Id = (int)EnumApprovalStatus.Rejected, Name = EnumApprovalStatus.Rejected.ToString() }
                );
        }
    }
}
