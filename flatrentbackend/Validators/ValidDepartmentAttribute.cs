using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FlatRent.Validators
{
    public class ValidDepartmentAttribute : ValidationAttribute
    {
        private static readonly string[] ValidDepartments = {"Sales", "CustomerService", "Supply", "Accounting"};

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var department = value as string;
                return ValidDepartments.Contains(department)
                    ? ValidationResult.Success
                    : new ValidationResult(
                        $"Department must be one of these values: {string.Join(", ", ValidDepartments)}.");
            }
            catch (Exception e)
            {
                return new ValidationResult("Invalid entry");
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, string.Join(", ", ValidDepartments));
        }
    }
}