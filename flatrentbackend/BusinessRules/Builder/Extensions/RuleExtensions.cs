using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder.Extensions
{
    public static class RuleExtensions
    {
        #region If

        public static IRule If(this IRule rule, IRuleCondition condition)
        {
            if (rule.If != null) throw new InvalidOperationException("If was already set");
            rule.If = condition;
            return rule;
        }

        public static IRule If(this IRule rule, Func<object, bool> condition)
        {
            rule.If(RuleCondition.FromFunc(condition));
            return rule;
        }

        public static IRule EndIf(this IRule rule)
        {
            if (rule.Else == null) rule.Else = RuleAction.DoNothing;
            return rule.Parent;
        }

        #endregion



        #region Then

        public static IRule Then(this IRule rule, IRuleAction thenAction)
        {
            if (rule.If == null) throw new InvalidOperationException("If was not set when calling Then");
            if (rule.Then != null) throw new InvalidOperationException("Then was already set");
            rule.Then = thenAction;
            return rule;
        }

        public static IRule Then(this IRule rule)
        {
            var newRule = new IfThenElseRule(rule);
            rule.Then(newRule);
            return newRule;
        }

        public static IRule ThenIf(this IRule rule, Func<object, bool> condition)
        {
            var newRule = new IfThenElseRule(rule);
            newRule.If(condition);

            rule.Then(newRule);

            return newRule;
        }

        public static IRule Then(this IRule rule, Func<object, object> action)
        {
            return rule.Then(RuleAction.FromFunc(action));
        }

        public static IRule Then(this IRule rule, Action<object> action)
        {
            return rule.Then(RuleAction.FromAct(action));
        }

        #endregion



        #region Else

        public static IRule Else(this IRule rule, IRuleAction action)
        {
            if (rule.Then == null) throw new InvalidOperationException("Then was not set when calling Else");
            if (rule.Else != null) throw new InvalidOperationException("Else was already set");
            rule.Else = action;
            return rule;
        }

        public static IRule Else(this IRule rule, Func<object, object> action)
        {
            return rule.Else(RuleAction.FromFunc(action));
        }

        public static IRule Else(this IRule rule, Action<object> action)
        {
            return rule.Else(RuleAction.FromAct(action));
        }

        public static IRule Else(this IRule rule)
        {
            var newRule = new IfThenElseRule(rule);
            rule.Else(newRule);
            return newRule;
        }

        public static IRule ElseIf(this IRule rule, Func<object, bool> condition)
        {
            var newRule = new IfThenElseRule(rule);
            newRule.If(condition);
            
            rule.Else(newRule);

            return newRule;
        }

        #endregion



        public static IRuleAction Build(this IRule rule)
        {
            var rootRule = rule;
            while (rootRule.Parent != null)
            {
                rootRule = rootRule.Parent;
            }

            return rootRule;
        }
    }
}