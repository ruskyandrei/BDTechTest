using HtmlAgilityPack;
using NUnit.Framework;
using Services.Providers;
using Services.Providers.Google;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ProvidersTests.GoogleTests
{
    public class GoogleScraperTests
    {
        private IScraper Scraper;
        private HtmlDocument HtmlPage;

        [SetUp]
        public void Setup()
        {
            Scraper = new GoogleScraper();
            HtmlPage = new HtmlDocument();

            HtmlPage.Load(@"ProvidersTests\GoogleTests\GoogleHtmlPage.txt");
        }

        [Test]
        public async Task ScrapingHtmlReturnsSearchResults()
        {
            var results = await Scraper.ScrapeResults(HtmlPage);

            Assert.IsTrue(results.Count() > 0);
        }

    }
}
