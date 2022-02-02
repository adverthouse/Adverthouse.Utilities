using Adverthouse.Common.Interfaces;

namespace Test.WebUI.Models
{
    public class Member : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int Age { get; set; }        
    }
}
