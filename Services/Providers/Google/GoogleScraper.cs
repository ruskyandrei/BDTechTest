using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Services.Enums;
using Services.Models;

namespace Services.Providers.Google
{
    public class GoogleScraper : IScraper
    {
        public SearchProvider SearchProvider => SearchProvider.Google;

        public async Task<IEnumerable<SearchResult>> ScrapeResults(HtmlDocument html)
        {
            var searchResults = new List<SearchResult>();

            if(html == null)
            {
                return searchResults;
            }

            var links = html.DocumentNode.SelectNodes("//div[@id='rso']//*[@class='r']/a");            

            if (links == null)
            {
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
