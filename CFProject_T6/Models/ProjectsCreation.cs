using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CFProject_T6.Models
{
    public partial class ProjectsCreation
    {
       public Projects Project { get; set; }
        public Packages packages { get; set; }
    }
}
