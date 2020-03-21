using HtmlAgilityPack;
using Services.Enums;
using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Providers
{
    public interface IScraper
    {
        SearchProvider SearchProvider { get; }

        Task<IEnumerable<SearchResult>> ScrapeResults(HtmlDocument html);
    }
}
