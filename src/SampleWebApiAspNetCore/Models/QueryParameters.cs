using System;
using System.Linq;

namespace SampleWebApiAspNetCore.Models
{
    public class QueryParameters
    {
        private const int maxPageCount = 50;
        public int Page { get; set; } = 1;

        private int _pageCount = maxPageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }
        
        public string Query { get; set; }

        public string OrderBy { get; set; } = "Name";
    }
}