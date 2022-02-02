using Adverthouse.Utility.Validation.Validators;

namespace Adverthouse.Utility.Validation
{
    public interface IRuleBuilder
    {
        PropertyRule ValidationRule { get; }
        void AddRule(IPropertyValidator validator);
    }
}
