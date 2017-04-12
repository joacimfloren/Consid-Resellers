using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class StoreViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Ett butiksnamn måste anges.")]
        [Display(Name = "Namn")]
        [MaxLength(100, ErrorMessage = "Butiksnamnet får inte vara längre än 100 tecken.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "En adress måste anges.")]
        [Display(Name = "Adress")]
        [MaxLength(512, ErrorMessage = "Adressen får inte vara längre än 512 tecken.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "En stad måste anges.")]
        [Display(Name = "Stad")]
        [MaxLength(512, ErrorMessage = "Stadsnamnet får inte vara längre än 512 tecken.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ett postnummer måste anges.")]
        [Display(Name = "Postnummer")]
        [MaxLength(16, ErrorMessage = "Postnummret får inte vara längre än 16 tecken.")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Ett land måste anges.")]
        [Display(Name = "Land")]
        [MaxLength(50, ErrorMessage = "Landsnamnet får inte vara längre än 50 tecken.")]
        public string Country { get; set; }

        [Display(Name = "Longitud")]
        [MaxLength(15, ErrorMessage = "Longituden får inte vara längre än 15 tecken.")]
        public string Longitude { get; set; }

        [Display(Name = "Latitud")]
        [MaxLength(15, ErrorMessage = "Latituden får inte vara längre än 15 tecken.")]
        public string Latitude { get; set; }

        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Ett företag måste väljas.")]
        public string CompanyId { get; set; }
    }
}
