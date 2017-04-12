using ConsidResellers.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class CompanyViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Ett företagsnamn måste anges.")]
        [Display(Name = "Namn")]
        [MaxLength(255, ErrorMessage = "Företagsnamnet får inte vara längre än 255 tecken.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ett organisationsnummer måste anges.")]
        [Display(Name = "Organisationsnummer")]
        public int? OrganizationNumber { get; set; }

        [Display(Name = "Noteringar")]
        public string Notes { get; set; }

        public List<StoreViewModel> Stores { get; set; }
    }
}
