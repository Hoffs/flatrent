using System;
using FlatRent.Models;

namespace FlatRent.BusinessRules
{
    public static class RuleChecker
    {
        public static (bool, FormError) Check(params Func<(bool, FormError)>[] rules)
        {
            foreach (var rule in rules)
            {
                var (passed, error) = rule();
                if (!passed)
                {
                    return (false, error);
                }
            }

            return (true, null);
        }
    }
}