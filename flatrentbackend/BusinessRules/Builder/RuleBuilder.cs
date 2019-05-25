using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public class RuleBuilder
    {
        public static IRule<TIn, TOut> If<TIn, TOut>(IRuleCondition<TIn> condition) where TIn : class where TOut : class
        {
            return new IfThenElseRule<TIn, TOut>(null, condition, null, null);
        }

        public static IRule<TIn, TOut> If<TIn, TOut>(Func<TIn, bool> condition) where TIn : class where TOut : class
        {
            return new IfThenElseRule<TIn, TOut>(null, RuleCondition.FromFunc(condition), null, null);
        }
    }
}