using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ProxyServiceTests
    {
        [Test]
        public async Task GetProxyReturnsAProxy()
        {
            //Arrange
            var proxyService = new ProxyService();

            //Act
            var proxy = await proxyService.GetProxy();

            //Assert
            Assert.IsTrue(proxy.Host != null && proxy.Port != 0);
        }
    }
}
