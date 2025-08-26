namespace NoumanJobPortal.Models.Users
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Skill[]? skills { get; set; }
        public required ApplicationUser User { get; set; }
    }
}
