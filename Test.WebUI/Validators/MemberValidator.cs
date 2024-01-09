using Adverthouse.Utility.Validation;
using Test.WebUI.Models;

namespace Test.WebUI.Validators
{
    public class MemberValidator : ValidatorBase<VMMember>
    {
        public MemberValidator(string formID,string password) : base(formID)
        {
            AddRule(a => a.FirstName).Required("First name required");
            AddRule(a => a.LastName).Required();

            AddRule(a => a.UserName).Required().Email();

            AddRule(a => a.Password).Required()
                                    .MinMaxLength(4, null, "Min length is 4 letter.");

            AddRule(a => a.Age).Required().Integer("Age must be numeric");

            AddRule(a=>a.Password2)
                .CompareValue("Password",password,"Password must be same with each other")
                .Required()
                .MinMaxLength(4, null, "Min length is 4 letter.");
        }
    }
}
