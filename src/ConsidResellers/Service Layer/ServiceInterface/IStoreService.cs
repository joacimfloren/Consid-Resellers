using ConsidResellers.Helpers;
using ConsidResellers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Service_Layer.ServiceInterface
{
    public interface IStoreService
    {
        Task<ServiceResponse<Store>> Get(string id);
        Task<ServiceResponse<Dictionary<List<Store>, int>>> GetAllWithPages(int? pageNumber, int? pageSize);
        ServiceResponse<Store> Add(Store store);
        ServiceResponse<Store> Update(Store store);
        Task<ServiceResponse<Store>> Remove(string id);
    }
}
