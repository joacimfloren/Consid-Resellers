using ConsidResellers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class CompanyListViewModel
    {
        public List<CompanyViewModel> Companies { get; set; }
        public Pager Pager { get; set; }
    }
}
