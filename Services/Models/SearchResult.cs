using Services.Enums;

namespace Services.Models
{
    public class SearchResult
    {
        public string Label { get; set; }

        public string Url { get; set; }

        public SearchProvider SearchEngine { get; set; }
    }
}
