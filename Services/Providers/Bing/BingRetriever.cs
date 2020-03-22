using System;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Services.Configuration;
using Services.Enums;
using Services.Models;

namespace Services.Providers.Bing
{
    public class BingRetriever : IRetriever
    {
        public SearchProvider SearchProvider => SearchProvider.Bing;

        private string SearchTerm = null;
        private string NextPageUrl = null;

        private readonly IConfig _config;
        private readonly ILogger<BingRetriever> _logger;
        private readonly string BaseAddress = "https://www.bing.com";

        public BingRetriever(IConfig config, ILogger<BingRetriever> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<HtmlDocument> RetrieveResultsFromProvider(string searchTerm)
        {
            var web = new HtmlWeb();
            web.UserAgent = _config.UserAgent;

            var doc = web.Load($"{BaseAddress}/search?q={searchTerm}");

            SearchTerm = searchTerm;
            NextPageUrl = GetLinkToNextPage(doc);            

            return doc;
        }

        public async Task<HtmlDocument> RetrieveResultsFromProviderNextPage()
        {
            if (NextPageUrl == null)
            {
                _logger.LogWarning("Couldn't find next page information");
                return null;
            }

            //random delay before repeat calls to avoid anti spam/bot lockdown
            await Task.Delay(new Random().Next(_config.RetrieveDelayMsMin, _config.RetrieveDelayMsMax));

            var web = new HtmlWeb();
            web.UserAgent = _config.UserAgent;

            var doc = web.Load($"{BaseAddress}{NextPageUrl}");

            NextPageUrl = GetLinkToNextPage(doc);

            return doc;
        }

        private string GetLinkToNextPage(HtmlDocument doc)
        {
            var navNode = doc.DocumentNode.SelectSingleNode($"//nav[@role='navigation' and @aria-label='More results for {SearchTerm}']/ul");

            var nextPageNode = navNode?.LastChild?.LastChild;

            if (nextPageNode == null)
            {
                return null;
            }

            var href = nextPageNode.Attributes["href"].Value;

            return HttpUtility.HtmlDecode(href);
        }
    }
}
