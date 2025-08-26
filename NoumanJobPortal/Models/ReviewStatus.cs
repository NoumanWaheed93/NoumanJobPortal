namespace NoumanJobPortal.Models
{
    /// <summary>
    /// The review status class represents the review status of a job application. (An application 
    /// can be reviewed by the employer)
    /// It is different from the approval status which is used for job postings (A job posting is reviewed by the admin when 
    /// it is posted, It becomes public if the admin approves it).
    /// </summary>
    public class ReviewStatus
    {
        public int Id { get; set; }
        public required string Name { get; set; } // e.g., Pending, Reviewed, Accepted, Rejected
    }
}
