using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace FlatRent.Controllers.Attributes
{
    public class ExactQueryParamAttribute: Attribute, IActionConstraint
    {
        private readonly string[] _keys;

        public ExactQueryParamAttribute(params string[] keys)
        {
            _keys = keys;
        }

        public bool Accept(ActionConstraintContext context)
        {
            var query = context.RouteContext.HttpContext.Request.Query;
            return query.Count == _keys.Length && _keys.All(key => query.ContainsKey(key));
        }

        public int Order => 0;
    }
}