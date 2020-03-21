using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Services.Enums;
using Services.Models;

namespace Services.Providers.DuckDuckGo
{
    public class DuckDuckGoScraper : IScraper
    {
        public SearchProvider SearchProvider => SearchProvider.DuckDuckGo;

        public async Task<IEnumerable<SearchResult>> ScrapeResults(HtmlDocument html)
        {
            var searchResults = new List<SearchResult>();

            if (html == null)
            {
                return searchResults;
            }

            var resultLabels = html.DocumentNode.SelectNodes("//table[comment()]/tr/td/a[@class='result-link']")?.ToArray();
            var resultLinks = html.DocumentNode.SelectNodes("//table[comment()]/tr/td/span[@class='link-text']")?.ToArray();

            if(resultLabels == null || resultLinks == null)
            {
                return searchResults;
            }

            for(int i=0; i < resultLabels.Length; i++)
            {
                var searchResult = new SearchResult();

                searchResult.Label = RemoveUnwantedTags(resultLabels[i].InnerText);
                searchResult.Url = resultLinks[i].InnerText;
                searchResult.SearchEngine = SearchProvider.DuckDuckGo;

                searchResults.Add(searchResult);
            }

            return searchResults;
        }

        //https://stackoverflow.com/questions/12787449/html-agility-pack-removing-unwanted-tags-without-removing-content
        private string RemoveUnwantedTags(string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;

            var document = new HtmlDocument();
            document.LoadHtml(data);

            var acceptableTags = new String[] { "strong", "em", "u" };

            var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                if (!acceptableTags.Contains(node.Name) && node.Name != "#text")
                {
                    var childNodes = node.SelectNodes("./*|./text()");

                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            nodes.Enqueue(child);
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }

            return document.DocumentNode.InnerHtml;
        }
    }
}
