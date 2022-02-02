namespace Adverthouse.Common.Interfaces
{
    public interface IPSFBase
    {
        string SortBy { get; set; }
        bool SortAscending { get; set; }
        string SortExpression { get; }
        int CurrentPage { get; set; }
        int ItemPerPage { get; set; }
        int TotalItemCount { get; set; }
        bool SetPageNumbers { get; set; }
        int PageCount { get; }
        string Filter { get; }
    }
}
