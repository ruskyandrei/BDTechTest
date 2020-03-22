using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Services;
using Services.Configuration;
using Services.Providers;
using Services.Providers.Bing;
using Services.Providers.DuckDuckGo;
using System.Threading.Tasks;

namespace Test.ProvidersTests.DuckDuckGoTests
{
    public class DuckDuckGoRetrieverTests
    {
        private IRetriever Retriever;

        [SetUp]
        public void Setup()
        {
            Retriever = new DuckDuckGoRetriever(new Config(), new Mock<ILogger<DuckDuckGoRetriever>>().Object);
        }


        [Test]
        public async Task ReturnsHtmlPageWhenSearchIsCalled()
        {
            //Arrange            

            //Act
            var htmlResult = await Retriever.RetrieveResultsFromProvider("test");

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("<title>test at DuckDuckGo</title>"));
        }

        [Test]
        public async Task ReturnsSecondHtmlPageWhenSearchNextIsCalled()
        {
            //Arrange

            //Act
            await Retriever.RetrieveResultsFromProvider("test");
            var htmlResult_page2 = await Retriever.RetrieveResultsFromProviderNextPage();

            //Assert
            Assert.IsTrue(htmlResult_page2.Text.Contains("<td valign=\"top\">20.&nbsp;</td>"));
        }

        [Test]
        public async Task CanSearchForMultipleWords()
        {
            //Arrange

            //Act
            var htmlResult = await Retriever.RetrieveResultsFromProvider("I like ice-cream");

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("<title>I like ice-cream at DuckDuckGo</title>"));
        }
    }
}
