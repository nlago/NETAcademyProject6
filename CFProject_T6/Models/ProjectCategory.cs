using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public class ProjectCategory
    {
        public List<Categories> Categories { get; set; }
        public List<ProjectSearchResultVM> Projects { get; set; }
    }
}
