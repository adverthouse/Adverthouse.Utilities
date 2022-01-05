using System;

namespace Test.WebUI.Models
{
    public static class CoreData
    {
        public static DateTime LastUpdateDate { get; set; } = DateTime.Now;
        public static int LockTIC = 0;
        public static int TotalItemCount = 0;
    }
}
