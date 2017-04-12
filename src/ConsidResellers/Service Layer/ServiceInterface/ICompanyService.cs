using ConsidResellers.Helpers;
using ConsidResellers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Service_Layer.ServiceInterface
{
    public interface ICompanyService
    {
        Task<ServiceResponse<Company>> Get(string id);
        Task<ServiceResponse<List<Company>>> GetAll();
        Task<ServiceResponse<Dictionary<Company, int>>> GetWithStores(string id, int? pageNumber, int? pageSize);
        Task<ServiceResponse<Dictionary<List<Company>, int>>> GetAllWithPages(int? pageNumber, int? pageSize);
        ServiceResponse<Company> Add(Company company);
        ServiceResponse<Company> Update(Company company);
        Task<ServiceResponse<Company>> Remove(string id);
    }
}
