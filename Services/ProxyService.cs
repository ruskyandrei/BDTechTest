using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Models;

namespace Services
{
    public class ProxyService : IProxyService
    {
        private string ProxyApiUrl = "https://api.getproxylist.com/proxy?protocol[]=http&allowsHttps=1&country[]=GB&country[]=CA&country[]=US";

        public async Task<Proxy> GetProxy()
        {
            var client = new HttpClient();
            var proxyResponseContentString = await (await client.GetAsync(ProxyApiUrl)).Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Proxy>(proxyResponseContentString);
        }
    }
}
