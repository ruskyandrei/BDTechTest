using Microsoft.Extensions.Logging;
using Moq;
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
            Retriever = new BingRetriever(new Config(), new Mock<ILogger<BingRetriever>>().Object);
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

        [Test]
        public async Task CanSearchForMultipleWords()
        {
            //Arrange

            //Act
            var htmlResult = await Retriever.RetrieveResultsFromProvider("I like ice-cream");

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("<title>I like ice-cream - Bing</title>"));
        }
    }
}
