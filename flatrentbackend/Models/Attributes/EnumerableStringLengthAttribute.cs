using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FlatRent.Models.Attributes
{
    public class EnumerableStringMaxLengthAttribute : ValidationAttribute
    {
        public int MaxLength { get; set; }

        private const string DefaultMessage = "All items must be shorter or equal to {1} characters.";

        public EnumerableStringMaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is IEnumerable<string> items))
                throw new ArgumentException(
                    $"Validation value is not {typeof(IEnumerable<string>)}, but {value.GetType()}.");

            if (items.All(item => item.Length <= MaxLength))
            {
                return ValidationResult.Success;
            }

            var template = string.IsNullOrEmpty(ErrorMessage) ? DefaultMessage : ErrorMessage;
            return new ValidationResult(string.Format(template, MaxLength));
        }
    }
}