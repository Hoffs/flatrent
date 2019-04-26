using System.Collections.Generic;

namespace FlatRent.Models
{
    public class ApiErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
        public string Message { get; set; }
    }
}