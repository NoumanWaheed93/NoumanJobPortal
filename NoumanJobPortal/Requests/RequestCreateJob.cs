namespace NoumanJobPortal.Requests
{
    public class RequestCreateJob
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<int> SkillIds { get; set; }
        public required int EmployerId { get; set; }

    }
}
