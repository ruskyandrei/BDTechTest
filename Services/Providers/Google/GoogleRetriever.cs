using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Services.Enums;

namespace Services.Providers.Google
{
    public class GoogleRetriever : IRetriever
    {
        public SearchProvider SearchProvider => SearchProvider.Google;

        private int PageNumber = 0;
        private int ResultsPerPage = 10;
        private string SearchTerm = null;

        private HttpClient HttpClient;

        public GoogleRetriever()
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri("http://www.google.com/");
        }

        public async Task<string> RetrieveResultsFromProvider(string searchTerm)
        {
            PageNumber = 1;
            SearchTerm = searchTerm;

            var result = await HttpClient.GetAsync($"search?q={HttpUtility.UrlEncode(searchTerm)}");
            var body = await result.Content.ReadAsStringAsync();

            return body;
        }

        public async Task<string> RetrieveResultsFromProviderNextPage()
        {
            var result = await HttpClient.GetAsync($"search?q={HttpUtility.UrlEncode(SearchTerm)}&start={PageNumber*ResultsPerPage}");
            var body = await result.Content.ReadAsStringAsync();

            PageNumber++;

            return body;
        }
    }
}
