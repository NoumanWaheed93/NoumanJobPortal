namespace NoumanJobPortal.Requests
{
    public class RequestCreateEmployer
    {
        public required string CompanyName { get; set; }

        public string? CompanyWebsite { get; set; }

        public required string UserId { get; set; }
    }
}
