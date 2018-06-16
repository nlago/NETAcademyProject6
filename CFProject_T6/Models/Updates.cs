using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class Updates
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Descr { get; set; }
        public DateTime Timestamp { get; set; }

        public Projects Project { get; set; }
    }
}
