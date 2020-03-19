using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Services.Enums;
using Services.Models;
using Services.Providers;
using Services.Configuration;

namespace Services
{
    public class SearchService : ISearchService
    {
        private readonly IEnumerable<IRetriever> _retrievers;
        private readonly IEnumerable<IScraper> _scrapers;
        private readonly IAggregatorService _aggregatorService;

        public SearchService(IEnumerable<IRetriever> retrievers, IEnumerable<IScraper> scrapers, IAggregatorService aggregatorService)
        {
            _retrievers = retrievers;
            _scrapers = scrapers;
            _aggregatorService = aggregatorService;
        }

        public async Task<IEnumerable<SearchResult>> Search(string searchTerm)
        {
            //get results from all enabled search providers
            var searchTasks = new List<Task<IEnumerable<SearchResult>>>();
            foreach (var searchProvider in Config.EnabledSearchProviders)
            {
                searchTasks.Add(SearchUsingSearchProvider(searchTerm, searchProvider));
            }
            var results = (await Task.WhenAll(searchTasks)).SelectMany(sr => sr);

            //aggregate results
            return await _aggregatorService.AggregateResults(results);
        }

        public async Task<IEnumerable<SearchResult>> SearchUsingSearchProvider(string searchTerm, SearchProvider searchProvider)
        {
            var searchResults = new List<SearchResult>();

            var retriever = _retrievers.SingleOrDefault(r => r.SearchProvider == searchProvider);
            if(retriever == null)
            {
                throw new ArgumentException($"No retriever found for search provider: {searchProvider.ToString()}");
            }

            var scraper = _scrapers.SingleOrDefault(s => s.SearchProvider == searchProvider);
            if (scraper == null)
            {
                throw new ArgumentException($"No scraper found for search provider: {searchProvider.ToString()}");
            }

            //retrieve data as html, start new search with this provider
            var htmlResult = await retriever.RetrieveResultsFromProvider(searchTerm);

            //scrape results from html
            var scrapedResults = await scraper.ScrapeResults(htmlResult);

            searchResults.AddRange(scrapedResults);

            //retrieve and scrape more pages until we have enough results
            while(searchResults.Count() < Config.NumberOfResults || scrapedResults.Count() > 0)
            {
                htmlResult = await retriever.RetrieveResultsFromProviderNextPage();
                scrapedResults = await scraper.ScrapeResults(htmlResult);
            }

            return searchResults;
        }

    }
}
