using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public class RuleBuilder
    {
        public static IRule If(IRuleCondition condition)
        {
            return new IfThenElseRule(null, condition, null, null);
        }

        public static IRule If(Func<object, bool> condition)
        {
            return new IfThenElseRule(null, RuleCondition.FromFunc(condition), null, null);
        }
    }
}