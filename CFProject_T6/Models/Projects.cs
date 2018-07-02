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

        }
        
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Descr { get; set; }
        [Required]
        public decimal Goalfunds { get; set; }
        public long CreatorId { get; set; }
        public decimal Fundsrecv { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Categories Category { get; set; }
        public Users Creator { get; set; }
        public ICollection<BackersProjects> BackersProjects { get; set; }
        public ICollection<Packages> Packages { get; set; }
        public ICollection<Photos> Photos { get; set; }
    }
}
