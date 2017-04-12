using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models
{
    public class Store
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        public string Address { get; set; }

        [Required]
        [MaxLength(512)]
        public string City { get; set; }

        [Required]
        [MaxLength(16)]
        public string Zip { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(15)]
        public string Longitude { get; set; }

        [MaxLength(15)]
        public string Latitude { get; set; }

        public virtual Company Company { get; set; }
    }
}
