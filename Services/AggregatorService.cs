using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Configuration;
using Services.Enums;
using Services.Models;

namespace Services
{
    public class AggregatorService : IAggregatorService
    {
        public class LinkSearchResultComparer : IEqualityComparer<SearchResult>
        {
            public bool Equals([AllowNull] SearchResult x, [AllowNull] SearchResult y)
            {
                return x.Url == y.Url;
            }

            public int GetHashCode([DisallowNull] SearchResult obj)
            {
                return obj.Url.GetHashCode();
            }
        }

        public async Task<IEnumerable<SearchResult>> AggregateResults(IEnumerable<IEnumerable<SearchResult>> searchResults)
        {
            var providerResults = searchResults.ToArray();

            if (providerResults.Length == 0)
            {
                return new List<SearchResult>();
            }

            var mergedResults = MergeDuplicateLinks(providerResults);

            List<SearchResult> aggregatedResults = AlternateProviderResults(mergedResults);

            return aggregatedResults;
        }

        private static List<SearchResult> AlternateProviderResults(List<SearchResult> mergedResults)
        {
            var singleProviderMergedResults = mergedResults.Where(mr => mr.SearchEngine.Count() == 1).ToList();
            var aggregatedResults = new List<SearchResult>();

            int i = 0;
            while (singleProviderMergedResults.Count() > 0)
            {
                SearchProvider provider = (SearchProvider)(i++ % 3);

                var getResult = singleProviderMergedResults.FirstOrDefault(r => r.SearchEngine.Contains(provider));
                if (getResult != null)
                {
                    singleProviderMergedResults.Remove(getResult);
                    aggregatedResults.Add(getResult);
                }
            }

            var multiProviderResults = mergedResults.Where(mr => mr.SearchEngine.Count() > 1).ToList();
            multiProviderResults.Sort((a, b) => a.SearchEngine.Count() - b.SearchEngine.Count());

            foreach (var multiProviderResult in multiProviderResults)
            {
                aggregatedResults.Insert(0, multiProviderResult);
            }

            return aggregatedResults;
        }

        private static List<SearchResult> MergeDuplicateLinks(IEnumerable<SearchResult>[] providerResults)
        {
            var mergedResults = new List<SearchResult>();
            mergedResults.AddRange(providerResults[0]);

            for (int i = 1; i < providerResults.Length; i++)
            {
                var commonLinks = providerResults[i].Intersect(mergedResults, new LinkSearchResultComparer());
                var commonLinksInAggregatedResults = mergedResults.Where(ar => commonLinks.Select(l => l.Url).Contains(ar.Url));

                foreach (var link in commonLinksInAggregatedResults)
                {
                    link.SearchEngine.Add(commonLinks.First().SearchEngine.First());
                }

                mergedResults.AddRange(providerResults[i].Except(commonLinks));
            }

            return mergedResults;
        }
    }
}
