using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IAggregatorService
    {
        Task<IEnumerable<SearchResult>> AggregateResults(IEnumerable<IEnumerable<SearchResult>> searchResults);
    }
}
