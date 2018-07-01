using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public class PackagesVM
    {
        public IEnumerable<CFProject_T6.Models.Packages> Packages {get; set; }
        public bool IsCreator { get; set; }
    }
}
