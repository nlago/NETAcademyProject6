using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class Users
    {
        public Users()
        {
            BackersProjects = new HashSet<BackersProjects>();
            Projects = new HashSet<Projects>();
        }

        public long Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<BackersProjects> BackersProjects { get; set; }
        public ICollection<Projects> Projects { get; set; }
    }
}
