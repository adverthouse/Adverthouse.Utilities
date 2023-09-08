namespace Adverthouse.Common.Data
{
    public class PSNoFilter : PSFBase
    {
        public override string Filter
        {
            get
            {
                return "";
            }
        }
        public PSNoFilter() { }

        public PSNoFilter(int currentPage,string sortBy,bool sortAscending = false)
        {
            CurrentPage = currentPage;
            SortBy = sortBy;
            SortAscending = sortAscending;
        }
    }
}
