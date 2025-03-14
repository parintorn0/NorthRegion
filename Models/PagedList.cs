using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthRegion.Models
{
    public class PagedList<NorthRegionViewModel>: List<NorthRegionViewModel> {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public PagedList(List<NorthRegionViewModel> items, int pageNumber, int pageSize, int totalRecords): base(items) {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}