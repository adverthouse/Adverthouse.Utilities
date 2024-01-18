namespace Adverthouse.Common.Data
{
    public class PSNoFilter : PSFBase
    {
        public override string Filter => string.Empty; 
        public PSNoFilter() { }

        public PSNoFilter(int currentPage,string sortBy,bool sortAscending = false)
        {
            CurrentPage = currentPage;
            SortBy = sortBy;
            SortAscending = sortAscending;
        }
    }
}
