using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IProxyService
    {
        Task<Proxy> GetProxy();
    }
}
