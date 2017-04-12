using ConsidResellers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class StoreListViewModel
    {
        public List<StoreViewModel> Stores { get; set; }
        public Pager Pager { get; set; }
    }
}
