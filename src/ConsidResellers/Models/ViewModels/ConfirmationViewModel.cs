using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models.ViewModels
{
    public class ConfirmationViewModel
    {
        public string Message { get; set; }
        public string RouteController { get; set; }
        public string RouteAction { get; set; }
    }
}
