using Services.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Services.Models
{
    public class SearchResult
    {
        public string Label { get; set; }

        public string Url { get; set; }

        public IList<SearchProvider> SearchEngine { get; set; }
    }

    public class SearchResultComparer : IEqualityComparer<SearchResult>
    {
        public bool Equals([AllowNull] SearchResult x, [AllowNull] SearchResult y)
        {
            return x.Url == y.Url
                && x.Label == y.Label
                && x.SearchEngine.Count() == y.SearchEngine.Count()
                && x.SearchEngine.Intersect(y.SearchEngine).Count() == x.SearchEngine.Count();
        }

        public int GetHashCode([DisallowNull] SearchResult obj)
        {
            return obj.Url.GetHashCode()
                + obj.Label.GetHashCode()
                + obj.SearchEngine.Select(se => se.GetHashCode()).Sum();
        }
    }
}
