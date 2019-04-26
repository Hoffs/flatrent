using System.Collections.Generic;

namespace FlatRent.Models
{
    public class FormError
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public FormError()
        {

        }

        public FormError(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public FormError(string message)
        {
            Name = "General";
            Message = message;
        }

        public static IEnumerable<FormError> CreateList(params FormError[] errors)
        {
            return errors;
        }
    }
}