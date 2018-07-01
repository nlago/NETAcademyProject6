using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public class PurchaseConfirmationModel
    {
        public bool ProjectIsLive { get; set; }

        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public long PackageId { get; set; }

        public string Username { get; set; }
        public string ProjectTitle { get; set; }
        public string Reward { get; set; }
        public decimal Amount { get; set; }

    }
}
