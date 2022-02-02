using Adverthouse.Utility.Validation.Validators;
using System;

namespace Adverthouse.Utility.Validation
{
    public class RuleBuilder : IRuleBuilder
    {
        public PropertyRule ValidationRule { get; }

        public RuleBuilder(string name, Type type)
        {
            ValidationRule = new PropertyRule(name, type);
        }

        public void AddRule(IPropertyValidator validator)
        {
            ValidationRule.PropertyValidator.Add(validator);
        }
    }
}
