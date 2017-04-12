using ConsidResellers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class CompanyFullViewModel
    {
        public CompanyViewModel Company { get; set; }
        public Pager StorePager { get; set; }
    }
}
