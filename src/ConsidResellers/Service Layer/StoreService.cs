using ConsidResellers.Models;
using ConsidResellers.Service_Layer.Base;
using ConsidResellers.Service_Layer.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsidResellers.Data_Layer;
using Microsoft.EntityFrameworkCore;
using ConsidResellers.Helpers;
using Microsoft.Extensions.Configuration;

namespace ConsidResellers.Service_Layer
{
    public class StoreService : BaseService<Store>, IStoreService
    {
        public StoreService(ConsidContext context, IConfiguration config) : base(context, config)
        {
        }

        public async Task<ServiceResponse<Store>> Get(string id)
        {
            ServiceResponse<Store> serviceResponse = new ServiceResponse<Store>();
            serviceResponse.Data = await _context.Stores.Where(s => s.Id == new Guid(id)).FirstOrDefaultAsync();

            if (serviceResponse.Data == null)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.Store.NotFound;
            }

            return serviceResponse;
        }

        public ServiceResponse<Store> Add(Store store)
        {
            ServiceResponse<Store> serviceResponse = new ServiceResponse<Store>();
            ValidationHelper validate = new ValidationHelper();

            if (store == null)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.NotAValidObject;
                return serviceResponse;
            }

            ValidationProperty[] validationProperties = new ValidationProperty[]
            {
                new ValidationProperty("Id", "NotAllowed"),
                new ValidationProperty("CompanyId", "Required|IsGuid"),
                new ValidationProperty("Name", "Required|IsString|Max:100"),
                new ValidationProperty("Address", "Required|IsString|Max:512"),
                new ValidationProperty("City", "Required|IsString|Max:512"),
                new ValidationProperty("Zip", "Required|IsString|Max:16"),
                new ValidationProperty("Country", "Required|IsString|Max:50"),
                new ValidationProperty("Longitude", "IsLongitude|Max:15"),
                new ValidationProperty("Latitude", "IsLatitude|Max:15")
            };

            if (validate.Object(store, validationProperties) && validate.validateLatLong(store.Latitude, store.Longitude))
            {
                try
                {
                    _context.Add(store);
                    _context.SaveChanges();
                    serviceResponse.Data = store;
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

        public async Task<ServiceResponse<Dictionary<List<Store>, int>>> GetAllWithPages(int? pageNumber, int? pageSize)
        {
            return await _getAllWithPages(_context.Stores.Include(c => c.Company), pageNumber, pageSize, "butiker");
        }

        public ServiceResponse<Store> Update(Store store)
        {
            ServiceResponse<Store> serviceResponse = new ServiceResponse<Store>();
            ValidationHelper validate = new ValidationHelper();

            if (store == null)
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = _error.NotAValidObject;
                return serviceResponse;
            }

            ValidationProperty[] validationProperties = new ValidationProperty[]
            {
                new ValidationProperty("Id", "Required|IsGuid"),
                new ValidationProperty("CompanyId", "Required|IsGuid"),
                new ValidationProperty("Name", "Required|IsString|Max:100"),
                new ValidationProperty("Address", "Required|IsString|Max:512"),
                new ValidationProperty("City", "Required|IsString|Max:512"),
                new ValidationProperty("Zip", "Required|IsString|Max:16"),
                new ValidationProperty("Country", "Required|IsString|Max:50"),
                new ValidationProperty("Longitude", "IsLongitude|Max:15"),
                new ValidationProperty("Latitude", "IsLatitude|Max:15")
            };

            if (validate.Object(store, validationProperties) && validate.validateLatLong(store.Latitude, store.Longitude))
            {
                var storeEntity = _context.Stores.Where(s => s.Id == store.Id).FirstOrDefault();
                if (storeEntity == null)
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.Store.NotFound;
                    return serviceResponse;
                }

                try
                {
                    storeEntity.Name = store.Name;
                    storeEntity.Address = store.Address;
                    storeEntity.Zip = store.Zip;
                    storeEntity.City = store.City;
                    storeEntity.CompanyId = store.CompanyId;
                    storeEntity.Country = store.Country;
                    storeEntity.Longitude = store.Longitude;
                    storeEntity.Latitude = store.Latitude;
                    _context.SaveChanges();

                    serviceResponse.Data = storeEntity;
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

        public async Task<ServiceResponse<Store>> Remove(string id)
        {
            ServiceResponse<Store> serviceResponse = new ServiceResponse<Store>();

            try
            {
                var store = await _context.Stores.Where(s => s.Id == new Guid(id)).FirstOrDefaultAsync();
                if (store == null)
                {
                    serviceResponse.ErrorOccured = true;
                    serviceResponse.Error = _error.Store.NotFound;
                    return serviceResponse;
                }
                try
                {
                    _context.Remove(store);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = store;
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
                serviceResponse.Error = _error.Store.CouldNotDelete;
            }

            return serviceResponse;
        }
    }
}
