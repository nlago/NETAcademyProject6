using System;
using System.Collections.Generic;

namespace CFProject_T6.Models
{
    public partial class Packages
    {
        public Packages()
        {
            BackersProjects = new HashSet<BackersProjects>();
        }

        public long Id { get; set; }
        public decimal DonationUpperlim { get; set; }
        public string Reward { get; set; }
        public long ProjectId { get; set; }

        public Projects Project { get; set; }
        public ICollection<BackersProjects> BackersProjects { get; set; }
    }
}
