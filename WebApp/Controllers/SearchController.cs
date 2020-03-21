using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Enums;
using Services.Models;

namespace WebApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IEnumerable<SearchResult>> Get([FromQuery] string searchTerm)
        {
            return await _searchService.Search(searchTerm);
        }
    }
}