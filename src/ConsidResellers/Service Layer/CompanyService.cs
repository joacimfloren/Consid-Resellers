using ConsidResellers.Data_Layer;
using ConsidResellers.Helpers;
using ConsidResellers.Models;
using ConsidResellers.Service_Layer.Base;
using ConsidResellers.Service_Layer.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Service_Layer
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(ConsidContext context, IConfiguration config) : base(context, config)
        {
        }

        public async Task<ServiceResponse<Company>> Get(string id)
        {
            ServiceResponse<Company> serviceResponse = new ServiceResponse<Company>();
            var company = await _context.Companies.Where(c => c.Id == new Guid(id)).FirstOrDefaultAsync();

            if (company == null)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.Company.NotFound;
            }

            serviceResponse.Data = company;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Dictionary<Company, int>>> GetWithStores(string id, int? pageNumber, int? pageSize)
        {
            ServiceResponse<Dictionary<Company, int>> serviceResponse = new ServiceResponse<Dictionary<Company, int>>();

            try
            {
                Page page = new Page(pageNumber, pageSize, _config);
                var company = await _context.Companies
                    .Where(c => c.Id == new Guid(id))
                    .FirstOrDefaultAsync();

                if (company == null)
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.Company.NotFound;
                    return serviceResponse;
                }

                try
                {
                    company.Stores = await _context.Stores.Where(s => s.CompanyId == new Guid(id)).Skip(page.PageNumber * page.PageSize - page.PageSize).Take(page.PageSize).ToListAsync();
                    var totalStoresCount = await _context.Stores.Where(s => s.CompanyId == new Guid(id)).CountAsync();

                    Dictionary<Company, int> responseData = new Dictionary<Company, int>();
                    responseData[company] = totalStoresCount;
                    serviceResponse.Data = responseData;
                }
                catch
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.Store.CouldNotGet;
                    return serviceResponse;
                }
            }
            catch
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.Company.CouldNotGet;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<Dictionary<List<Company>, int>>> GetAllWithPages(int? pageNumber, int? pageSize)
        {
            return await _getAllWithPages(_context.Companies, pageNumber, pageSize, "företag");
        }

        public ServiceResponse<Company> Add(Company company)
        {
            ServiceResponse<Company> serviceResponse = new ServiceResponse<Company>();
            ValidationHelper validate = new ValidationHelper();

            if (company == null)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.NotAValidObject;
                return serviceResponse;
            }

            ValidationProperty[] validationProperties = new ValidationProperty[]
            {
                new ValidationProperty("Id", "NotAllowed"),
                new ValidationProperty("Name", "Required|IsString|Max:255"),
                new ValidationProperty("OrganizationNumber", "Required|IsInteger")
            };

            if (validate.Object(company, validationProperties))
            {
                try
                {
                    _context.Add(company);
                    _context.SaveChangesAsync();
                    serviceResponse.Data = company;
                }
                catch
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.DatabaseError;
                    return serviceResponse;
                }
            }
            else
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = validate.GetErrors();
            }

            return serviceResponse;
        }

        public ServiceResponse<Company> Update(Company company)
        {
            ServiceResponse<Company> serviceResponse = new ServiceResponse<Company>();
            ValidationHelper validate = new ValidationHelper();

            if (company == null)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.NotAValidObject;
                return serviceResponse;
            }

            ValidationProperty[] validationProperties = new ValidationProperty[]
            {
                new ValidationProperty("Id", "Required|IsGuid"),
                new ValidationProperty("Name", "Required|IsString|Max:255"),
                new ValidationProperty("OrganizationNumber", "Required|IsInteger")
            };

            if (validate.Object(company, validationProperties)) {
                var companyEntity = _context.Companies.Where(c => c.Id == company.Id).FirstOrDefault();
                if (companyEntity == null)
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.Company.NotFound;
                    return serviceResponse;
                }

                try
                {
                    companyEntity.Name = company.Name;
                    companyEntity.OrganizationNumber = company.OrganizationNumber;
                    companyEntity.Notes = company.Notes;
                    _context.SaveChanges();

                    serviceResponse.Data = companyEntity;
                }
                catch
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.DatabaseError;
                    return serviceResponse;
                }
            }
            else
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = validate.GetErrors();
            }               

            return serviceResponse;
        }

        public async Task<ServiceResponse<Company>> Remove(string id)
        {
            ServiceResponse<Company> serviceResponse = new ServiceResponse<Company>();

            try
            {
                var company = await _context.Companies.Where(c => c.Id == new Guid(id)).FirstOrDefaultAsync();
                if (company == null)
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.Company.NotFound;
                    return serviceResponse;
                }

                try
                {
                    var stores = await _context.Stores.Where(s => s.CompanyId == new Guid(id)).ToListAsync();
                    if (stores.Count > 0)
                    {
                        foreach (var store in stores)
                        {
                            _context.Remove(store);
                        }
                    }

                    _context.Remove(company);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = company;
                }
                catch
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.DatabaseError;
                    return serviceResponse;
                }
            }
            catch
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.Company.CouldNotDelete;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Company>>> GetAll()
        {
            ServiceResponse<List<Company>> serviceResponse = new ServiceResponse<List<Company>>();

            try
            {
                serviceResponse.Data = await _context.Companies.ToListAsync();
            }
            catch (Exception)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.DatabaseError;
            }

            return serviceResponse;
        }
    }
}
