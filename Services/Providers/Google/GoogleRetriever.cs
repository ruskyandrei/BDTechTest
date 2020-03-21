﻿using System;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Services.Configuration;
using Services.Enums;

namespace Services.Providers.Google
{
    public class GoogleRetriever : IRetriever
    {
        public SearchProvider SearchProvider => SearchProvider.Google;

        private string NextPageUrl = null;
        private readonly string BaseAddress = "https://www.google.com";
        private readonly IConfig _config;

        public GoogleRetriever(IConfig config)
        {
            _config = config;
        }

        public async Task<HtmlDocument> RetrieveResultsFromProvider(string searchTerm)
        {
            var web = new HtmlWeb();
            web.UserAgent = _config.UserAgent;

            var doc = web.Load($"{BaseAddress}/search?q={searchTerm}");
            
            NextPageUrl = GetLinkToNextPage(doc);

            return doc;
        }

        public async Task<HtmlDocument> RetrieveResultsFromProviderNextPage()
        {
            if(NextPageUrl==null)
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
            var nextPageNode = doc.DocumentNode.SelectSingleNode("//a[@id='pnnext']");

            if(nextPageNode == null)
            {
                return null;
            }

            var href = nextPageNode.Attributes["href"].Value;

            return HttpUtility.HtmlDecode(href);
        }
    }
}
