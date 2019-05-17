using System;
using System.ComponentModel.DataAnnotations;

namespace GalaxyBuildersSystem.Models
{
    public class GTask
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public DateTime DateAssigned { get; set; }
        public bool IsDone { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}