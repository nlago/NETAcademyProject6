using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CFProject_T6.Models
{
    public partial class Projects
    {
        public Projects()
        {
            BackersProjects = new HashSet<BackersProjects>();
            Packages = new HashSet<Packages>();
            Photos = new HashSet<Photos>();
            Updates = new HashSet<Updates>();
            Videos = new HashSet<Videos>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public decimal Goalfunds { get; set; }
        public long CreatorId { get; set; }
        public decimal Fundsrecv { get; set; }
        public long CategoryId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Categories Category { get; set; }
        public Users Creator { get; set; }
        public ICollection<BackersProjects> BackersProjects { get; set; }
        public ICollection<Packages> Packages { get; set; }
        public ICollection<Photos> Photos { get; set; }
        public ICollection<Updates> Updates { get; set; }
        public ICollection<Videos> Videos { get; set; }
    }
}
