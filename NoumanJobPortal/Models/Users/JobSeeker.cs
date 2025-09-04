namespace NoumanJobPortal.Models.Users
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public ICollection<Skill> skills { get; set; } = null!;
        public required ApplicationUser User { get; set; }
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
    }
}
