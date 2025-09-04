using NoumanJobPortal.Models.Users;

namespace NoumanJobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required Employer Employer { get; set; }
        public required ApprovalStatus ApprovalStatus { get; set; }
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
}
