using NUnit.Framework;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services.Enums;
using Services;
using System.Linq;

namespace Test
{
    public class AggregatorServiceTests
    {
        private IAggregatorService Service { get; set; }

        [SetUp]
        public void Setup()
        {
            Service = new AggregatorService();
        }

        [Test]
        public async Task ReturnsAggregatedResults()
        {
            //Arrange
            var google_results = new List<SearchResult>()
            {
                new SearchResult()
                {
                    Label = "google_test_result_1",
                    Url = "www.google-url1.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                },
                new SearchResult()
                {
                    Label = "google_test_result_2",
                    Url = "www.google-url2.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                },
                new SearchResult()
                {
                    Label = "google_test_result_3",
                    Url = "www.same-url.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                },
                new SearchResult()
                {
                    Label = "google_test_result_4",
                    Url = "www.google-url4.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                }
            };
            var bing_results = new List<SearchResult>()
            {
                new SearchResult()
                {
                    Label = "bing_test_result_1",
                    Url = "www.bing-url1.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                },
                new SearchResult()
                {
                    Label = "bing_test_result_2",
                    Url = "www.same-url.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                },
                new SearchResult()
                {
                    Label = "bing_test_result_3",
                    Url = "www.bing-url3.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                },
                new SearchResult()
                {
                    Label = "bing_test_result_4",
                    Url = "www.bing-url4.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                }
            };
            var ddg_results = new List<SearchResult>()
            {
                new SearchResult()
                {
                    Label = "ddg_test_result_1",
                    Url = "www.same-url.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                },
                new SearchResult()
                {
                    Label = "ddg_test_result_2",
                    Url = "www.ddg-url2.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                },
                new SearchResult()
                {
                    Label = "ddg_test_result_3",
                    Url = "www.ddg-url3.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                },
                new SearchResult()
                {
                    Label = "ddg_test_result_4",
                    Url = "www.ddg-url4.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                }
            };

            var expectedResults = new List<SearchResult>()
            {
                new SearchResult()
                {
                    Label = "google_test_result_3",
                    Url = "www.same-url.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google, SearchProvider.Bing, SearchProvider.DuckDuckGo }
                },
                new SearchResult()
                {
                    Label = "google_test_result_1",
                    Url = "www.google-url1.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                },
                new SearchResult()
                {
                    Label = "bing_test_result_1",
                    Url = "www.bing-url1.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                },
                new SearchResult()
                {
                    Label = "ddg_test_result_2",
                    Url = "www.ddg-url2.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                },
                new SearchResult()
                {
                    Label = "google_test_result_2",
                    Url = "www.google-url2.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                },
                new SearchResult()
                {
                    Label = "bing_test_result_3",
                    Url = "www.bing-url3.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                },
                new SearchResult()
                {
                    Label = "ddg_test_result_3",
                    Url = "www.ddg-url3.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                },
                new SearchResult()
                {
                    Label = "google_test_result_4",
                    Url = "www.google-url4.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Google }
                },
                new SearchResult()
                {
                    Label = "bing_test_result_4",
                    Url = "www.bing-url4.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.Bing }
                },
                new SearchResult()
                {
                    Label = "ddg_test_result_4",
                    Url = "www.ddg-url4.com",
                    SearchEngine = new List<SearchProvider>() { SearchProvider.DuckDuckGo }
                }
            };

            //Act
            var aggregatedResults = await Service.AggregateResults(new List<IEnumerable<SearchResult>>()
            {
                google_results,
                bing_results,
                ddg_results
            });

            //Assert
            Assert.IsTrue(aggregatedResults.Except(expectedResults, new SearchResultComparer()).Count() == 0);
        }
    }
}
