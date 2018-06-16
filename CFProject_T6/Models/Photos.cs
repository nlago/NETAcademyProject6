using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class Photos
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Filename { get; set; }

        public Projects Project { get; set; }
    }
}
