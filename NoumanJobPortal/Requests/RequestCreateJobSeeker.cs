namespace NoumanJobPortal.Requests
{
    public class RequestCreateJobSeeker
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserId { get; set; }
        public int[]? Skills { get; set; } = null;
    }
}
