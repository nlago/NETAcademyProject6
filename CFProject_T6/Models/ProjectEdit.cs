using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFProject_T6.Models
{
    public class ProjectEdit
    {
        public Projects Project { get; set; }
        public IFormFile Photo { get; set; }
    }
}
