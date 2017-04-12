using AutoMapper;
using ConsidResellers.Models;
using ConsidResellers.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Helpers
{
    public static class MapHelper
    {
        private static MapperConfiguration mapConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<Company, CompanyViewModel>();
                cfg.CreateMap<Store, StoreViewModel>();
                cfg.CreateMap<CompanyViewModel, Company>();
                cfg.CreateMap<StoreViewModel, Store>();
        });

        public static IMapper getMapConfig()
        {
            return mapConfig.CreateMapper();
        }
    }
}
