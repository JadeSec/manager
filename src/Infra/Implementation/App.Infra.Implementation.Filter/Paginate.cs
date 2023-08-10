using System;
using System.Linq;
using System.Collections.Generic;
using App.Infra.Implementation.Filter.Interfaces;

namespace App.Infra.Implementation.Filter
{
    public class Paginate<T> : IPaginate
    {
        private int _maxPerPage { get; set; }

        private int _totalPages => (int)Math.Ceiling((double)Total / _maxPerPage);

        public int Next => Page < _totalPages ? Page + 1 : -1;

        public int Prev => Page > 1 ? Page - 1 : -1;

        public int Page { get; set; }

        public int Total { get; set; }
        
        public int StartIndex => (_maxPerPage * (Page - 1)) + 1;

        public int EndIndex => Math.Min(_maxPerPage * Page, Total);

        public List<T> Items { get; set; }

        public Filter Filter { get; set; }

        public Paginate(Filter filter, List<T> items, int total)
        {
            _maxPerPage = filter.Configuration.MaxPerPage;
            
            Total = total;
            Items = items;
            Filter = filter;
            Page = filter.CurrentPage;
        }

        public int[] Pages
        {
            get
            {
                const int maxVisiblePages = 5;
                int middlePage = Math.Min(Page, _totalPages);

                int startPage = Math.Max(middlePage - maxVisiblePages / 2, 1);
                int endPage = Math.Min(startPage + maxVisiblePages - 1, _totalPages);

                return Enumerable.Range(startPage, endPage - startPage + 1).ToArray();
            }
        }
    }
}
