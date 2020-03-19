using Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Configuration
{
    public static class Config
    {
        public static readonly IEnumerable<SearchProvider> EnabledSearchProviders = new List<SearchProvider>() { SearchProvider.Google, SearchProvider.Bing, SearchProvider.DuckDuckGo };
        public static readonly int NumberOfResults = 100;
        public static readonly int RetrieveDelayMs = 500;
    }
}
