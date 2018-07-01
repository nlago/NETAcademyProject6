using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public class ProjectSearchResultVM
    {
        public Projects Project { get; set; }
        public Photos Photo { get; set; }
        public bool IsCreator { get; set; }
    }
}
