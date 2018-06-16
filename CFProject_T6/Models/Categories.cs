using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Projects = new HashSet<Projects>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Projects> Projects { get; set; }
    }
}
