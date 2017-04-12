using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Models
{
    public class ServiceResponse<T>
    {
        public bool ErrorOccured { get; set; } = false;
        public object Error { get; set; } = null;
        public T Data { get; set; } = default(T);
    }
}
