using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBuildersSystem.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        public decimal Productivity { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<GTask> Tasks { get; set; }
    }
}