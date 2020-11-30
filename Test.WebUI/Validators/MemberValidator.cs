using Adverthouse.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.WebUI.Models;

namespace Test.WebUI.Validators
{
    public class MemberValidator : ValidatorBase<Member>
    {
        public MemberValidator(string formID) : base(formID)
        {
            AddRule(a => a.FirstName).Required();
            AddRule(a => a.LastName).Required();

            AddRule(a => a.UserName).Required(); 

            AddRule(a => a.Password).Required()
                                  .MinMaxLength(4, null, "Min length is 4 letter.");

            AddRule(a => a.Age).Required().
                Integer("Age must be numeric");
        }
    }
}
