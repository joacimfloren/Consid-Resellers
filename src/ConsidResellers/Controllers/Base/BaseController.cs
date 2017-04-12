using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ConsidResellers.Helpers;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ConsidResellers.Controllers.Base
{
    public class BaseController : Controller
    {
        protected static IMapper _mapper;
        protected static IConfiguration _config;

        public BaseController(IConfiguration config)
        {
            _mapper = MapHelper.getMapConfig();
            _config = config;
        }
    }
}
