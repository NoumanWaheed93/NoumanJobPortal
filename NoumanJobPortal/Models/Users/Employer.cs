
namespace NoumanJobPortal.Models.Users
{
    public class Employer
    {
        public int Id { get; set; }
        public required string CompanyName { get; set; }
        public string? CompanyWebsite { get; set; }
        public Job[]? PostedJobs { get; set; }

        public required ApplicationUser User { get; set; }
    }
}
