using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RecrutimentApp.Utilities
{
    public class GreaterThanZeroAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal? dec = (decimal?)value;
            if (dec.HasValue)
            {
                if (dec.Value <= 0)
                {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val", "true");
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val-greaterthanzero", GetErrorMessage(context.ModelMetadata.DisplayName));
        }

        private string GetErrorMessage(string displayName)
        {
            return $"Field \"{displayName}\" must have value greater than 0";
        }
    }

    public class NotPastDate : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? date = value as DateTime?;
            if (date.HasValue)
            {
                if (date.Value <= DateTime.Now)
                {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val", "true");
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val-notpastdate", GetErrorMessage(context.ModelMetadata.DisplayName));
        }

        private string GetErrorMessage(string displayName)
        {
            return $"The {displayName} field can not have past date";
        }
    }

    public class AdultAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? date = value as DateTime?;
            if (date.HasValue)
            {
                if (date.Value.Date.AddYears(18) > DateTime.Now)
                {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val", "true");
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val-adult", GetErrorMessage(context.ModelMetadata.DisplayName));
        }

        private string GetErrorMessage(string displayName)
        {
            return $"You must have at least 18 years to apply for a job";
        }
    }

    public class MoneyNotGreaterThanAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string propertyNameToCompare;
        public MoneyNotGreaterThanAttribute(string propertyNameToCompare) => this.propertyNameToCompare = propertyNameToCompare;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyName = validationContext.ObjectType.GetProperty(propertyNameToCompare);
            if (propertyName == null)
                throw new ArgumentException($"Unknow property {propertyNameToCompare}");

            var propertyValue = propertyName.GetValue(validationContext.ObjectInstance, null) as decimal?;
            decimal? thisFieldValue = value as decimal?;

            var displayAttribute = propertyName.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;

            if (propertyValue.HasValue && thisFieldValue.HasValue)
            {
                if (thisFieldValue > propertyValue)
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName, displayAttribute?.GetName() ?? propertyNameToCompare));
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        { 
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val", "true");
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val-moneynotgreaterthan",
                GetErrorMessage(context.ModelMetadata.DisplayName, propertyNameToCompare));
            ValidationAttributesHelpers.MergeAttribute(context.Attributes, "data-val-moneynotgreaterthan-otherattribute", propertyNameToCompare);
        }

        private string GetErrorMessage(string thisField, string otherField)
        {
            return $"The {thisField} field can not have greater value than {otherField} field";
        }
    }

    public static class ValidationAttributesHelpers
    {
        public static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
