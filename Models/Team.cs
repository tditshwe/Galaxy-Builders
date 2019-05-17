using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyBuildersSystem.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Productivity { get; set; }
        public virtual ICollection<Employee> Employees { get; set;  }
    }
}