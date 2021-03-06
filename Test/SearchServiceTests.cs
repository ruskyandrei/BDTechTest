﻿using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Services;
using Services.Configuration;
using Services.Enums;
using Services.Models;
using Services.Providers;
using Services.Providers.Bing;
using Services.Providers.DuckDuckGo;
using Services.Providers.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SearchServiceTests
    {
        public class TestConfig : IConfig
        {
            public IEnumerable<SearchProvider> EnabledSearchProviders => new List<SearchProvider>() 
            { 
                SearchProvider.Google, 
                SearchProvider.DuckDuckGo, 
                SearchProvider.Bing 
            };

            public int NumberOfResults => 30;

            public int RetrieveDelayMsMin => 1000;

            public int RetrieveDelayMsMax => 5000;

            public string UserAgent => "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";
        }

        private readonly IConfig Config = new TestConfig();

        [Test]
        public async Task FullSearchTest()
        {
            //Arrange
            var retrievers = new List<IRetriever>()
            {
                new GoogleRetriever(Config,new Mock<ILogger<GoogleRetriever>>().Object),
                new DuckDuckGoRetriever(Config, new Mock<ILogger<DuckDuckGoRetriever>>().Object),
                new BingRetriever(Config, new Mock<ILogger<BingRetriever>>().Object)
            };

            var scrapers = new List<IScraper>()
            {
                new GoogleScraper(new Mock<ILogger<GoogleScraper>>().Object),
                new DuckDuckGoScraper(new Mock<ILogger<DuckDuckGoScraper>>().Object),
                new BingScraper(new Mock<ILogger<BingScraper>>().Object)
            };

            var aggregatorService = new AggregatorService();

            var searchService = new SearchService(retrievers, scrapers, aggregatorService, Config, new Mock<ILogger<SearchService>>().Object);
            //Act
            var results = await searchService.Search("tesla");
            //Assert

            Assert.IsTrue(results.Count() > 0);            
        }
    }
}
