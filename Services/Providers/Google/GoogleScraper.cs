using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Services.Enums;
using Services.Models;

namespace Services.Providers.Google
{
    public class GoogleScraper : IScraper
    {
        public SearchProvider SearchProvider => SearchProvider.Google;

        private readonly ILogger<GoogleScraper> _logger;

        public GoogleScraper(ILogger<GoogleScraper> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<SearchResult>> ScrapeResults(HtmlDocument html)
        {
            var searchResults = new List<SearchResult>();

            if(html == null || html.DocumentNode == null)
            {
                _logger.LogWarning("No html document to scrape.");
                return searchResults;
            }

            var links = html.DocumentNode.SelectNodes("//div[@id='rso']//*[@class='r']/a");            

            if (links == null)
            {
                _logger.LogWarning("Result links not found.");
                return searchResults;
            }

            foreach(var link in links)
            {
                var searchResult = new SearchResult();

                searchResult.Url = HttpUtility.HtmlDecode(link.Attributes["href"].Value);
                searchResult.Label = HttpUtility.HtmlDecode(link.ChildNodes["h3"] != null ? link.ChildNodes["h3"].InnerText : link.InnerText);
                searchResult.SearchEngine = new List<SearchProvider>() { SearchProvider.Google };

                searchResults.Add(searchResult);
            }

            return searchResults;
        }
    }
}
