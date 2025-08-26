namespace NoumanJobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required ApprovalStatus ApprovalStatus { get; set; }
    }
}
