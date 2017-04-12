using ConsidResellers.Data_Layer;
using ConsidResellers.Helpers;
using ConsidResellers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Service_Layer.Base
{
    public class BaseService<T> where T : class
    {
        protected ConsidContext _context;
        protected IConfiguration _config;
        protected ErrorMessage _error;

        public BaseService(ConsidContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _error = new ErrorMessage();
        }

        protected async Task<ServiceResponse<Dictionary<List<T>, int>>> _getAllWithPages(IQueryable<T> query, int? pageNumber, int? pageSize, string modelUrlName)
        {
            ServiceResponse<Dictionary<List<T>, int>> serviceResponse = new ServiceResponse<Dictionary<List<T>, int>>();

            try
            {
                Dictionary<List<T>, int> responseData = new Dictionary<List<T>, int>();
                Page page = new Page(pageNumber, pageSize, _config);

                var objects = await query.Skip(page.PageNumber * page.PageSize - page.PageSize)
                    .Take(page.PageSize)
                    .ToListAsync();

                var countObjects = await query.CountAsync();

                responseData[objects] = countObjects;
                serviceResponse.Data = responseData;
            }
            catch
            {
                serviceResponse.ErrorOccured = true;
                serviceResponse.Error = "Kunde inte hämta " + modelUrlName + ".";
            }

            return serviceResponse;
        }
    }
}
