using NUnit.Framework;
using Services;
using Services.Configuration;
using Services.Providers;
using Services.Providers.Bing;
using Services.Providers.DuckDuckGo;
using System.Threading.Tasks;

namespace Test.ProvidersTests.BingTests
{
    public class BingRetrieverTests
    {
        private IRetriever Retriever;

        [SetUp]
        public void Setup()
        {
            Retriever = new BingRetriever(new Config());
        }


        [Test]
        public async Task ReturnsHtmlPageWhenSearchIsCalled()
        {
            //Arrange            

            //Act
            var htmlResult = await Retriever.RetrieveResultsFromProvider("test");

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("<title>test - Bing</title>"));
        }

        [Test]
        public async Task ReturnsSecondPageHtmlPageWhenSearchNextIsCalled()
        {
            //Arrange

            //Act
            await Retriever.RetrieveResultsFromProvider("test");
            var htmlResult = await Retriever.RetrieveResultsFromProviderNextPage();

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("aria-label=\"Page 1\""));            
        }
    }
}
