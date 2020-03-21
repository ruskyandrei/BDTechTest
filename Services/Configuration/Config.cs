using Services.Enums;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Configuration
{
    public class Config : IConfig
    {
        public IEnumerable<SearchProvider> EnabledSearchProviders => new List<SearchProvider>() { SearchProvider.Google, SearchProvider.DuckDuckGo, SearchProvider.Bing };

        public int NumberOfResults => 30;

        public int RetrieveDelayMsMin => 1000;

        public int RetrieveDelayMsMax => 5000;

        public string UserAgent => "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";

    }
}
