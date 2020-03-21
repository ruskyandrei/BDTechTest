using HtmlAgilityPack;
using Services.Enums;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers
{
    public interface IRetriever
    {
        SearchProvider SearchProvider { get; }

        /// <summary>
        /// Returns the html body of the response from a new search against a provider
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        Task<HtmlDocument> RetrieveResultsFromProvider(string searchTerm);

        /// <summary>
        /// Returns the html body of the response from the next page of a search against a provider
        /// </summary>
        /// <returns></returns>
        Task<HtmlDocument> RetrieveResultsFromProviderNextPage();
    }
}
