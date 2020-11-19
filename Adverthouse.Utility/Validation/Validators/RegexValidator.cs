using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Adverthouse.Utility.Validation.Validators
{
    public class RegexValidator : IPropertyValidator
    {
        public string Pattern { get; set; }
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage == null ? $"{ProperyName} not valid." : _errorMessage;
            }
        }
        public string ProperyName { get; }

        public bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return false;
            }
            value = value.ToString().Trim();
            return !Regex.IsMatch(value.ToString(), Pattern, RegexOptions.IgnoreCase);
        }
        public RegexValidator(string propertName)
        {
            ProperyName = propertName;
        }
        public RegexValidator(string propertName, string errorMessage)
        {
            ProperyName = propertName;
            _errorMessage = errorMessage;
        }
    }
}
