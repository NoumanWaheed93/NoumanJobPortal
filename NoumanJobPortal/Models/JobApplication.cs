using NoumanJobPortal.Models.Users;

namespace NoumanJobPortal.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public required Job Job { get; set; }
        public required JobSeeker Applicant { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string? ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
        public required ReviewStatus ReviewStatus { get; set; } // e.g., Pending, Reviewed, Accepted, Rejected
    }
}
