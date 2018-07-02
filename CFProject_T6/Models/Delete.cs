using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public class Delete
    {
        public Projects Project { get; set; }
        public List<Packages> Packages { get; set; }
        public List<Photos> Photos { get; set; }
        public List<BackersProjects> BackersProjects { get; set; }
    }
}
