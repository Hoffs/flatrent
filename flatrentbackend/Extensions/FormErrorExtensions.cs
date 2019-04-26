using System.Collections.Generic;
using FlatRent.Models;

namespace FlatRent.Extensions
{
    public static class FormErrorExtensions
    {
        public static Dictionary<string, List<string>> GetFormattedResponse(this IEnumerable<FormError> errors)
        {
            var dict = new Dictionary<string, List<string>>();
            foreach (var formError in errors)
            {
                var exists = dict.TryGetValue(formError.Name, out var list);
                if (!exists)
                {
                    list = new List<string> {formError.Message};
                    dict.TryAdd(formError.Name, list);
                }
                else
                {
                    list.Add(formError.Message);
                }
            }

            return dict;
        }
    }
}