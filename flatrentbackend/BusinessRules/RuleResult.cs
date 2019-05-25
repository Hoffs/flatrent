using FlatRent.Models;

namespace FlatRent.BusinessRules
{
    public class RuleResult
    {
        public static readonly RuleResult Success = new RuleResult(true);
        public static RuleResult Failed(FormError f) => new RuleResult(false, f);

        private RuleResult(bool passed, FormError error = null)
        {
            Passed = passed;
            Error = error;
        }

        public bool Passed { get; }
        public FormError Error { get; }
    }
}