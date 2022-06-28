using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArtSolutions.MUN.Web.Helpers
{

    public class DecimalNumber : RegularExpressionAttribute, IClientValidatable
    {
        public DecimalNumber() :
                base(@"^[\.0-9]\d*(\.\d+)?$")
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (Regex.IsMatch(value.ToString(), Pattern, RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(Resources.Global.ValDecimalValue);
                }
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRegexRule(this.ErrorMessageString, Pattern);
            return new[] { rule };
        }
    }

    public class DecimalWithNegative : RegularExpressionAttribute, IClientValidatable
    {
        public DecimalWithNegative() :
                base(@"^-?[\.0-9]\d*(\.\d+)?$")
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (Regex.IsMatch(value.ToString(), Pattern, RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(Resources.Global.ValDecimalValue);
                }
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRegexRule(this.ErrorMessageString, Pattern);
            return new[] { rule };
        }
    }
}