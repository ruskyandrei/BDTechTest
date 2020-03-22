using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Services.Providers;
using Services.Providers.Bing;
using Services.Providers.DuckDuckGo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ProvidersTests.DuckDuckGoTests
{
    public class DuckDuckGoScraperTests
    {
        private IScraper Scraper;
        private HtmlDocument HtmlPage;

        [SetUp]
        public void Setup()
        {
            Scraper = new DuckDuckGoScraper(new Mock<ILogger<DuckDuckGoScraper>>().Object);
            HtmlPage = new HtmlDocument();

            HtmlPage.Load(@"ProvidersTests\DuckDuckGoTests\DuckDuckGoHtmlPage.txt");
        }

        [Test]
        public async Task ScrapingHtmlReturnsSearchResults()
        {
            var results = await Scraper.ScrapeResults(HtmlPage);

            Assert.IsTrue(results.Count() > 0);
        }

    }
}
