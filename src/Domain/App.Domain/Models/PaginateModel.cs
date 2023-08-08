using System.Collections.Generic;

namespace App.Domain.Models
{
    public class PaginateModel<T>
    {
        public int Next => Page < Total ? Page + 1 : -1;

        public int Prev => Page > 1 ? Page - 1 : -1;

        public int[] Pages
        {
            get
            {
                int[] pages = new int[Total];

                for (int i = 0; i < Total; i++)
                {
                    pages[i] = i + 1;
                }
                return pages;
            }
        }

        public int Page { get; set; }

        public int Total { get; set; }

        public List<T> Items { get; set; }

        public PaginateModel(int page, int total, List<T> items)
        {
            Page = page;
            Total = total;
            Items = items;
        }
    }
}