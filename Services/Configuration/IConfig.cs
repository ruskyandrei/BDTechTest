using Services.Enums;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Configuration
{
    public interface IConfig
    {
        IEnumerable<SearchProvider> EnabledSearchProviders { get; }
        int NumberOfResults { get; }
        int RetrieveDelayMsMin { get; }
        int RetrieveDelayMsMax { get; }
        string UserAgent { get; }
    }
}
