using Adverthouse.Utility.Validation.Validators;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Adverthouse.Utility.Validation
{
    public class ValidatorBase<T>
    {
        public string FormID { get; private set; }

        private bool _isValid;
        public List<ValidationError> ValidationErrors { get; private set; }
        public List<RuleBuilder> ValidationRules { get; private set; }

        public ValidatorBase(string formID)
        {
            FormID = formID;
            _isValid = false;
            ValidationRules = new List<RuleBuilder>();
            ValidationErrors = new List<ValidationError>();
        }

        public IRuleBuilder AddRule<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            string name = (expression.Body as MemberExpression).Member.Name;
            var type = (expression.Body as MemberExpression).Type;

            var ruleBuilder = new RuleBuilder(name, type);
            ValidationRules.Add(ruleBuilder);

            return ruleBuilder;
        }

        public bool IsValid(T entity)
        {
            _isValid = true;
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (RuleBuilder rule in ValidationRules)
            {
                var prop = props.Where(a => a.Name == rule.ValidationRule.PropertyName).FirstOrDefault();
                var value = prop.GetValue(entity);
                foreach (IPropertyValidator validator in rule
                    .ValidationRule.PropertyValidator)
                {
                    var isValid = validator.IsValid(value);
                    if (!isValid)
                    {
                        ValidationErrors.Add(new ValidationError(validator.ErrorMessage, rule.ValidationRule.PropertyName));
                    }
                    _isValid = _isValid != false && isValid;
                }
            }
            return _isValid;
        }

        public HtmlString GetValidationScript()
        {
            _isValid = false;
            var validationCode = string.Empty;
            var messageItems = string.Empty;
            var ruleItems = string.Empty;

            validationCode += "$(\"" + FormID + "\").validate({\r\n";
            var k = 0;

            foreach (RuleBuilder rule in ValidationRules)
            {
                k++;
                validationCode += "               ";

                ruleItems += $" {rule.ValidationRule.PropertyName}: {{ \r\n";
                messageItems += $" {rule.ValidationRule.PropertyName}: {{ \r\n";
                string ruleItem = "";
                string messageItem = "";
                foreach (IPropertyValidator item in rule.ValidationRule.PropertyValidator)
                {
                    ruleItem += (String.IsNullOrWhiteSpace(ruleItem) ? "" : ",");
                    messageItem += (String.IsNullOrWhiteSpace(ruleItem) ? "" : ",");
                    ruleItem += item.ScriptRule;
                    messageItem += item.ScriptMessage; 
                }
                ruleItems += ruleItem;
                messageItems += messageItem;
                ruleItems += $"}}" + (ValidationRules.Last() != rule ? "," : "") + "\r\n";
                messageItems += $"}}" + (ValidationRules.Last() != rule ? "," : "") + "\r\n";
            }
            validationCode += "         rules : { \r\n";
            validationCode += ruleItems;
            validationCode += "         },\r\n";
            validationCode += "         messages : { \r\n";
            validationCode += messageItems;
            validationCode += "        }\r\n";
            validationCode += "});";
            return new HtmlString(validationCode);
        }

        public HtmlString GetValidationErrors()
        {
            if (ValidationErrors.Count() > 0)
            {
                var temp = "";
                temp += "<ul class=\"alert alert-danger list-unstyled\">";
                temp += String.Format("<li><h3 class=\"text-danger\"><i class=\"fa fa-exclamation-triangle\"></i>{0}</h3></li>", "Validation error");
                foreach (ValidationError el in ValidationErrors)
                {
                    temp += String.Format("<li><strong>{0} : </strong> {1}</li>", el.ErrorField, el.ErrorMessage);
                }
                temp += "</ul>";
                return new HtmlString(temp);
            }
            else
                return new HtmlString(string.Empty);
        }
    }
}