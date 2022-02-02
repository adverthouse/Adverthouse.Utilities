using Adverthouse.Utility.Validation.Validators;

namespace Adverthouse.Utility.Validation
{
    public static class RoleBuilderOptions
    {
        public static IRuleBuilder NotNull(this IRuleBuilder ruleBuilder)
        {
            ruleBuilder.AddRule(new NotNullValidator(ruleBuilder.ValidationRule.PropertyName));
            return ruleBuilder;
        }
        public static IRuleBuilder NotNull(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new NotNullValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage));
            return ruleBuilder;
        }

        public static IRuleBuilder Required(this IRuleBuilder ruleBuilder)
        {
            ruleBuilder.AddRule(new RequiredValidator(ruleBuilder.ValidationRule.PropertyName));
            return ruleBuilder;
        }
        public static IRuleBuilder Required(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new RequiredValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage));
            return ruleBuilder;
        }

        public static IRuleBuilder MaxLength(this IRuleBuilder ruleBuilder, int maxLength)
        {
            ruleBuilder.AddRule(new MinMaxLengthValidator(null, maxLength, ruleBuilder.ValidationRule.PropertyName));
            return ruleBuilder;
        }
        public static IRuleBuilder MinMaxLength(this IRuleBuilder ruleBuilder,
            int? minLenght, int? maxLength)
        {
            ruleBuilder.AddRule(new MinMaxLengthValidator(minLenght, maxLength, ruleBuilder.ValidationRule.PropertyName));
            return ruleBuilder;
        }
        public static IRuleBuilder MinMaxLength(this IRuleBuilder ruleBuilder,
           int? minLenght, int? maxLength, string errorMessage)
        {
            ruleBuilder.AddRule(new MinMaxLengthValidator(minLenght, maxLength, ruleBuilder.ValidationRule.PropertyName, errorMessage));
            return ruleBuilder;
        }
        public static IRuleBuilder Email(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new EmailValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage));
            return ruleBuilder;
        }
        public static IRuleBuilder CompareValue(this IRuleBuilder ruleBuilder,
            string comparePropertyName, string errorMessage)
        {
            ruleBuilder.AddRule(new CompareValidator(ruleBuilder.ValidationRule.PropertyName, comparePropertyName, errorMessage));
            return ruleBuilder;
        }
        public static IRuleBuilder IsChecked(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new CheckValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage));
            return ruleBuilder;
        }
        public static IRuleBuilder Float(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new RegexValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage)
            {
                Pattern = @"^((\d+(\.\d{1,2})?)|((\d*\.)?\d{1,2}?))$"
            });
            return ruleBuilder;
        }
        public static IRuleBuilder Integer(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new RegexValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage)
            {
                Pattern = @"^\d+$"
            });
            return ruleBuilder;
        }
        public static IRuleBuilder OnlyLetterAndNumbers(this IRuleBuilder ruleBuilder, string errorMessage)
        {
            ruleBuilder.AddRule(new RegexValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage)
            {
                Pattern = @"^a-zA-Z0-9_\.$"
            });
            return ruleBuilder;
        }
        public static IRuleBuilder Regex(this IRuleBuilder ruleBuilder, string pattern, string errorMessage)
        {
            ruleBuilder.AddRule(new RegexValidator(ruleBuilder.ValidationRule.PropertyName, errorMessage)
            {
                Pattern = pattern
            });
            return ruleBuilder;
        }
    }
}