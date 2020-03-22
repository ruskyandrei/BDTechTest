using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Services.Enums;
using Services.Models;

namespace Services.Providers.Bing
{
    public class BingScraper : IScraper
    {
        private readonly ILogger _logger;

        public SearchProvider SearchProvider => SearchProvider.Bing;

        public BingScraper(ILogger<BingScraper> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<SearchResult>> ScrapeResults(HtmlDocument html)
        {
            var searchResults = new List<SearchResult>();

            if (html == null || html.DocumentNode == null)
            {
                _logger.LogWarning("No html document to scrape.");
                return searchResults;
            }

            var linkNodes = html.DocumentNode.SelectNodes("//ol[@id='b_results']/li/h2/a");

            foreach (var link in linkNodes)
            {
                var searchResult = new SearchResult();

                searchResult.Url = HttpUtility.HtmlDecode(link.Attributes["href"].Value);
                searchResult.Label = HttpUtility.HtmlDecode(link.InnerText);
                searchResult.SearchEngine = new List<SearchProvider>() { SearchProvider.Bing };

                searchResults.Add(searchResult);
            }

            return searchResults;
        }
    }
}
