using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class BackersProjects
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public long PackageId { get; set; }

        public Packages Package { get; set; }
        public Projects Project { get; set; }
        public Users User { get; set; }
    }
}
