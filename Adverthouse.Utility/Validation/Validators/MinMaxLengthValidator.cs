using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation.Validators
{
    public class MinMaxLengthValidator : IPropertyValidator
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage == null ? $"{ProperyName} error." : _errorMessage;
            }
        }
        public string ScriptRule{
            get {
                if (_minLength.HasValue && _maxLength.HasValue)
                {
                    return $" minlength : {_minLength.Value} \r\n," +
                           $" maxlength : {_maxLength.Value} \r\n";
                }
                else {
                    return $" { (_minLength.HasValue ? "minlength": "maxlength" )} :" +
                        $" {(_minLength.HasValue ? _minLength.Value : _maxLength.Value )} \r\n";
                }
            }
        }

        public string ScriptMessage {
            get
            {
                if (_minLength.HasValue && _maxLength.HasValue)
                {
                    return $" minlength : {ErrorMessage.Replace("\"", "'")} \r\n," +
                           $" maxlength : {ErrorMessage.Replace("\"", "'")} \r\n";
                }
                else
                {
                    return $" { (_minLength.HasValue ? "minlength" : "maxlength")} :" +
                        $" {(_minLength.HasValue ? ErrorMessage.Replace("\"", "'") : ErrorMessage.Replace("\"", "'") )} \r\n";
                }
            }
        }

        public string ProperyName { get; }

        private int? _minLength, _maxLength;
        public bool IsValid(object value)
        {
            bool _isValid = true;
            if (_minLength.HasValue)
            {
                if (Convert.ToString(value).Length < _minLength)
                {
                    _isValid = false;
                }
            }
            if (_maxLength.HasValue)
            {
                if (Convert.ToString(value).Length > _maxLength)
                {
                    _isValid = false;
                }
            }
            return _isValid;
        }

        public MinMaxLengthValidator(int? minLength, int? maxLength, string propertName)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            ProperyName = propertName;
        }

        public MinMaxLengthValidator(int? minLength, int? maxLength, string propertName, string errorMessage)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            ProperyName = propertName;
            _errorMessage = errorMessage;
        }
    }
}
