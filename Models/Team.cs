using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalaxyBuildersSystem.Models
{
    public class TeamRanking
    {
        public int Rank { get; set; }
        public string Description { get; set; }
        public decimal Productivity { get; set; }
        public Employee BestEmployee { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Productivity { get; set; }
        public virtual ICollection<Employee> Employees { get; set;  }
    }
}