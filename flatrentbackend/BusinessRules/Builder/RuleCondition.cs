using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public static class RuleCondition
    {
        public static IRuleCondition<TIn> FromFunc<TIn>(Func<TIn, bool> condition)
        {
            return new SimpleCondition<TIn>(condition);
        }

        class SimpleCondition<TIn> : IRuleCondition<TIn>
        {
            private readonly Func<TIn, bool> _condition;

            public SimpleCondition(Func<TIn, bool> condition)
            {
                _condition = condition;
            }

            public bool IsTrue(TIn ob)
            {
                return _condition(ob);
            }
        }
    }
}