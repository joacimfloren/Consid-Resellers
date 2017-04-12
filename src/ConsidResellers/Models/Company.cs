using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models
{
    public class Company
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int OrganizationNumber { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
