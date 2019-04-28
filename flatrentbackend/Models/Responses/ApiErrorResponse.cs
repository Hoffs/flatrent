using System.Collections.Generic;

namespace FlatRent.Models.Responses
{
    public class ApiErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
        public string Message { get; set; }
    }
}