using System;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Services.Configuration;
using Services.Enums;
using Services.Models;

namespace Services.Providers.DuckDuckGo
{
    public class DuckDuckGoRetriever : IRetriever
    {
        public SearchProvider SearchProvider => SearchProvider.DuckDuckGo;

        private string NextPageUrl = null;

        private IConfig _config;
        private readonly IProxyService _proxyService;
        private readonly string BaseAddress = "https://duckduckgo.com";

        public DuckDuckGoRetriever(IConfig config)
        {
            _config = config;
        }

        public async Task<HtmlDocument> RetrieveResultsFromProvider(string searchTerm)
        {
            var web = new HtmlWeb();
            web.UserAgent = _config.UserAgent;

            var doc = web.Load($"{BaseAddress}/lite?q={searchTerm}");

            NextPageUrl = GetLinkToNextPage(doc);

            return doc;
        }

        public async Task<HtmlDocument> RetrieveResultsFromProviderNextPage()
        {
            if (NextPageUrl == null)
            {
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
            var nextPageCommentNode = doc.DocumentNode.SelectSingleNode($"//form[@class='next_form' and @action='/lite/']/comment()");

            if (nextPageCommentNode == null)
            {
                return null;
            }

            var href = nextPageCommentNode.InnerHtml.Replace("<!-- <a rel=\"next\" href=\"", "").Replace("\">Next Page &gt;</a> //-->", "");

            return href;
        }
    }
}
