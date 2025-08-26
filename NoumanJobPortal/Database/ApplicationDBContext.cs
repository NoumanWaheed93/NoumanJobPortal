using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoumanJobPortal.Models;
using NoumanJobPortal.Models.Users;

namespace NoumanJobPortal.Database
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }


        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
        
            builder.Entity<ReviewStatus>()
                .HasData(
                    new ReviewStatus { Id = 1, Name = "Pending" },
                    new ReviewStatus { Id = 2, Name = "Reviewed" },
                    new ReviewStatus { Id = 4, Name = "Accepted" },
                    new ReviewStatus { Id = 3, Name = "Rejected" }
                );

            builder.Entity<ApprovalStatus>()
                .HasData(
                    new ApprovalStatus { Id = 1, Name = "Pending" },
                    new ApprovalStatus { Id = 2, Name = "Approved" },
                    new ApprovalStatus { Id = 3, Name = "Rejected" }
                );
        }
    }
}
