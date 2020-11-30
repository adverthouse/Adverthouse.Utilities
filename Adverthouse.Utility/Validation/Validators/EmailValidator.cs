using System;
using System.Text.RegularExpressions;

namespace Adverthouse.Utility.Validation.Validators
{
    public class EmailValidator : IPropertyValidator
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage == null ? $"{ProperyName} not a valid email" : _errorMessage;
            }
        }
        public string ScriptRule => $" email : true \r\n";
        public string ScriptMessage => $" email :\"{ErrorMessage.Replace("\"", "'")}\" \r\n";

        public string ProperyName { get; }

        public bool IsValid(object value)
        {
            var isValid = true;
            var emailFilter = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var illegalChars = @"[\(\)\<\>\,\;\:\\\""\[\]]";
            if (string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                isValid = false;
            }
            if (isValid)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(value)))
                {
                    var isEmail = Regex.IsMatch(value.ToString(), emailFilter, RegexOptions.IgnoreCase);
                    var isContainIllegal = Regex.IsMatch(value.ToString(), illegalChars, RegexOptions.IgnoreCase);
                    if (isEmail && !isContainIllegal)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        public EmailValidator(string propertName)
        {
            ProperyName = propertName;
        }
        public EmailValidator(string propertName, string errorMessage)
        {
            ProperyName = propertName;
            _errorMessage = errorMessage;
        }
    }
}
