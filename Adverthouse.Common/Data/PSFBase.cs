using Adverthouse.Common.Interfaces;
using System;
using System.Reflection.Metadata;

namespace Adverthouse.Common.Data
{
    /// <summary>
    ///  Paging, Sorting and Filtering base class
    /// </summary>
    public abstract class PSFBase : IPSFBase
    {
        public static int MAX_ITEM_PER_PAGE = 100;
        private int _itemPerPage = 20;

        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public string SortExpression
        {
            get
            {
                return SortAscending ? SortBy + " asc" : SortBy + " desc";
            }
        }
        public int CurrentPage { get; set; }
        public int ItemPerPage { 
            get { 
               return _itemPerPage;
            } 
            set {
                _itemPerPage = Math.Min(MAX_ITEM_PER_PAGE, _itemPerPage);
            } 
        }
        public int TotalItemCount { get; set; }
        public bool SetPageNumbers { get; set; }
        public int PageCount
        {
            get
            {
                return (TotalItemCount / ItemPerPage) + ((TotalItemCount % ItemPerPage) == 0 ? 0 : 1);
            }
        }
        public abstract string Filter { get; }
        public PSFBase(string sortBy, bool sortAscending = false, int currentPage = 1, int itemPerPage = 20, bool setPageNumbers = true)
        {
            CurrentPage = currentPage;
            ItemPerPage = itemPerPage;
            SortAscending = sortAscending;
            SortBy = sortBy;
            SetPageNumbers = setPageNumbers;
        }

        public PSFBase()
        {
            CurrentPage = 1;
            ItemPerPage = 20;
            SortAscending = false;
            SetPageNumbers = true;
        }
    }
}
