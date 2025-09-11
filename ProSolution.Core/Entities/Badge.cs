﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.Core.Entities
{
    public class Badge : BaseEntity
    {
       
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public bool IsSertificate { get; set; }

    }
}
