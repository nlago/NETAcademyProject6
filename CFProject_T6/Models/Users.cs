using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class Users:IdentityUser<long>
    {
        public Users()
        {
            BackersProjects = new HashSet<BackersProjects>();
            Projects = new HashSet<Projects>();
        }

        public string Fname { get; set; }
        public string Lname { get; set; }
        //public string ProfileUrl { get; set; }

        public ICollection<BackersProjects> BackersProjects { get; set; }
        public ICollection<Projects> Projects { get; set; }
    }
}
