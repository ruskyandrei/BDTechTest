using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services.Enums;
using Services.Models;

namespace Services.Providers.Google
{
    public class GoogleScraper : IScraper
    {
        public SearchProvider SearchProvider => SearchProvider.Google;

        public Task<IEnumerable<SearchResult>> ScrapeResults(string htmlBody)
        {
            throw new NotImplementedException();
        }
    }
}
