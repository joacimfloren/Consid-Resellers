using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class StoreAddViewModel
    {
        public StoreViewModel Store { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
    }
}
