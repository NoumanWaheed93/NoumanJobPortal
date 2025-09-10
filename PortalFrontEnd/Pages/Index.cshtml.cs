using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PortalFrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnJobSeekerClick()
        {
            Console.WriteLine("Job Seeker button clicked");
        }

        public void OnEmployerClick()
        {
            Console.WriteLine("Employer button clicked");
        }
    }
}
