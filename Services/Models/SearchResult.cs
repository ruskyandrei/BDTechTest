using Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class SearchResult
    {
        public string Label { get; set; }
        public string Description { get; set; }

        public string Url { get; set; }

        public SearchProvider SearchEngine { get; set; }
    }
}
