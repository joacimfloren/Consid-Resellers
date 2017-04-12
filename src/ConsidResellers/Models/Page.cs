using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models
{
    public class Page
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Page(int? pageNumber, int? pageSize, IConfiguration config)
        {
            int pNumber = 1;
            int pSize = Convert.ToInt32(config["Data:PageSize"]);

            if (pageNumber != null && pageNumber > 0) pNumber = (int)pageNumber;
            if (pageSize != null && pageSize > 0) pSize = (int)pageSize;
            
            PageNumber = pNumber;
            PageSize = pSize;
        }
    }
}
