using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Services.Enums;
using Services.Models;

namespace Services.Providers.Bing
{
    public class BingScraper : IScraper
    {
        public SearchProvider SearchProvider => SearchProvider.Bing;

        public async Task<IEnumerable<SearchResult>> ScrapeResults(HtmlDocument html)
        {
            var searchResults = new List<SearchResult>();

            if (html == null)
            {
                return searchResults;
            }

            var linkNodes = html.DocumentNode.SelectNodes("//ol[@id='b_results']/li/h2/a");

            foreach (var link in linkNodes)
            {
                var searchResult = new SearchResult();

                searchResult.Url = HttpUtility.HtmlDecode(link.Attributes["href"].Value);
                searchResult.Label = HttpUtility.HtmlDecode(link.InnerText);
                searchResult.SearchEngine = SearchProvider.Bing;

                searchResults.Add(searchResult);
            }

            return searchResults;
        }
    }
}
