using Adverthouse.Common.Data;

namespace Test.WebUI.PSFs
{
    public class PSFMember : PSFBase
    {
        public string FFirstName { get; set; }
        public string FLastName { get;set; }
        public override string Filter {
          get {
                return "";
            }
        }

        public PSFMember()
        {
            SortBy = "MemberID";
        }
    }
}
