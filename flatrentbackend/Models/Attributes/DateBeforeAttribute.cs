using System;
using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models.Attributes
{
    public class DateBeforeAttribute : ValidationAttribute
    {
        public int DaysToAdd { get; set; }

        private const string DefaultMessage = "Date must be before {1}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var dt = (DateTime) value;
                var from = DateTime.Now.Date.AddDays(DaysToAdd);
                // Might cause issues around midnight on date difference when date is higher than 
                if (dt.Date < from)
                {
                    return ValidationResult.Success;
                }

                var template = string.IsNullOrEmpty(ErrorMessage) ? DefaultMessage : ErrorMessage;
                return new ValidationResult(string.Format(template, validationContext.MemberName, from.ToString("yyyy-MM-dd")));
            }
            catch
            {
                return new ValidationResult("Klaida");
            }
        }
    }
}