namespace NoumanJobPortal.Models
{
    /// <summary>
    /// The approval status class represents the approval status of a job posting. (A job posting
    /// is reviewed by the admin when it is posted, It becomes public if the admin approves it).
    /// It is different from the review status which is used for job applications (An application 
    /// can be reviewed by the employer)
    /// </summary>
    public class ApprovalStatus
    {
        public int Id { get; set; }
        public required string Name { get; set; } // e.g., Pending, Approved, Rejected
    }
}
