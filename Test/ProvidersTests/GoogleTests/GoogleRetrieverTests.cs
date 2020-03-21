using HtmlAgilityPack;
using NUnit.Framework;
using Services.Configuration;
using Services.Providers;
using Services.Providers.Bing;
using Services.Providers.Google;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test.ProvidersTests.GoogleTests
{
    public class GoogleRetrieverTests
    {
        private IRetriever Retriever;

        [SetUp]
        public void Setup()
        {
            Retriever = new GoogleRetriever(new Config());
        }


        [Test]
        public async Task ReturnsHtmlPageWhenSearchIsCalled()
        {
            //Arrange
            

            //Act
            var htmlResult = await Retriever.RetrieveResultsFromProvider("test");

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("<title>test - Google Search</title>"));
        }

        [Test]
        public async Task ReturnsSecondHtmlPageWhenSearchNextIsCalled()
        {
            //Arrange


            //Act
            await Retriever.RetrieveResultsFromProvider("test");
            var htmlResult = await Retriever.RetrieveResultsFromProviderNextPage();

            //Assert
            Assert.IsTrue(htmlResult.Text.Contains("<a aria-label=\"Page 1\""));
        }
    }
}
