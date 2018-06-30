using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CFProject_T6.Models
{
    public class ProjectsCreation
    {
        public Projects Project { get; set; }
        public Packages Packages { get; set; }
        public IFormFile Photo { get; set; }
    }
}
