using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public static class RuleCondition
    {
        public static IRuleCondition FromFunc(Func<object, bool> condition)
        {
            return new SimpleCondition(condition);
        }

        class SimpleCondition : IRuleCondition
        {
            private readonly Func<object, bool> _condition;

            public SimpleCondition(Func<object, bool> condition)
            {
                _condition = condition;
            }

            public bool IsTrue(object ob)
            {
                return _condition(ob);
            }
        }
    }
}