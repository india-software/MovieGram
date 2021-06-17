using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Model
{
    public class PageListModel<T> : List<T>
    {
        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public int TotalRecords { get; private set; }

        public PageListModel(List<T> items, int pageIndex, int totalPages, int totalRecords)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
