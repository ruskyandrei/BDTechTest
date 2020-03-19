using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> Search(string searchTerm);
    }
}
