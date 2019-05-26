using System;
using FlatRent.BusinessRules.Builder.Interfaces;
using FlatRent.Models;

namespace FlatRent.BusinessRules.Builder.Extensions
{
    public static class RuleExtensions
    {
        #region If

        public static IRule<TIn, TOut> If<TIn, TOut>(this IRule<TIn, TOut> rule, IRuleCondition<TIn> condition) where TIn : class where TOut : class
        {
            if (rule.If != null) throw new InvalidOperationException("If was already set");
            rule.If = condition;
            return rule;
        }

        public static IRule<TIn, TOut> If<TIn, TOut>(this IRule<TIn, TOut> rule, Func<TIn, bool>  condition) where TIn : class where TOut : class
        {
            rule.If(RuleCondition.FromFunc(condition));
            return rule;
        }

        public static IRule<TIn, TOut> EndIf<TIn, TOut>(this IRule<TIn, TOut> rule) where TIn : class where TOut : class
        {
            if (rule.Else == null) rule.Else = RuleAction.DoNothing<TIn, TOut>();
            return rule.Parent;
        }

        #endregion



        #region Then

        public static IRule<TIn, TOut> Then<TIn, TOut>(this IRule<TIn, TOut> rule, IRuleAction<TIn, TOut> thenAction) where TIn : class where TOut : class
        {
            if (rule.If == null) throw new InvalidOperationException("If was not set when calling Then");
            rule.Then = thenAction;
            return rule;
        }

        public static IRule<TIn, TOut> Then<TIn, TOut>(this IRule<TIn, TOut> rule) where TIn : class where TOut : class
        {
            var newRule = new IfThenElseRule<TIn, TOut>(rule);
            rule.Then(newRule);
            return newRule;
        }

        public static IRule<TIn, TOut> ThenIf<TIn, TOut>(this IRule<TIn, TOut> rule, Func<TIn, bool>  condition) where TIn : class where TOut : class
        {
            var newRule = new IfThenElseRule<TIn, TOut>(rule);
            newRule.If(condition);

            rule.Then(newRule);

            return newRule;
        }

        public static IRule<TIn, TOut> Then<TIn, TOut>(this IRule<TIn, TOut> rule, Func<TIn, TOut> action) where TIn : class where TOut : class
        {
            return rule.Then(RuleAction.FromFunc(action));
        }

        public static IRule<TIn, TOut> Then<TIn, TOut>(this IRule<TIn, TOut> rule, Action<TIn> action) where TIn : class where TOut : class
        {
            return rule.Then(RuleAction.FromAct<TIn, TOut>(action));
        }

        #endregion



        #region Else

        public static IRule<TIn, TOut> Else<TIn, TOut>(this IRule<TIn, TOut> rule, IRuleAction<TIn, TOut> action) where TIn : class where TOut : class
        {
            if (rule.Then == null) throw new InvalidOperationException("Then was not set when calling Else");
            rule.Else = action;
            return rule;
        }

        public static IRule<TIn, TOut> Else<TIn, TOut>(this IRule<TIn, TOut> rule, Func<TIn, TOut> action) where TIn : class where TOut : class
        {
            return rule.Else(RuleAction.FromFunc(action));
        }

        public static IRule<TIn, TOut> Else<TIn, TOut>(this IRule<TIn, TOut> rule, Action<TIn> action) where TIn : class where TOut : class
        {
            return rule.Else(RuleAction.FromAct<TIn, TOut>(action));
        }

        public static IRule<TIn, TOut> Else<TIn, TOut>(this IRule<TIn, TOut> rule) where TIn : class where TOut : class
        {
            var newRule = new IfThenElseRule<TIn, TOut>(rule);
            rule.Else(newRule);
            return newRule;
        }

        public static IRule<TIn, TOut> ElseIf<TIn, TOut>(this IRule<TIn, TOut> rule, Func<TIn, bool>  condition) where TIn : class where TOut : class
        {
            var newRule = new IfThenElseRule<TIn, TOut>(rule);
            newRule.If(condition);
            
            rule.Else(newRule);

            return newRule;
        }

        #endregion

        #region DoAction

        public static IRule<TIn, TOut> Do<TIn, TOut>(this IRule<TIn, TOut> rule, Action<TIn> action) where TIn : class where TOut : class
        {
            rule.DoAction = action;
            return rule;
        }

        #endregion

        #region StopAction

        public static IRule<TIn, RuleResult> ReturnThen<TIn>(this IRule<TIn, RuleResult> rule, bool result, FormError error = null) where TIn : class 
        {
            rule.Then = RuleAction.FromFunc<TIn, RuleResult>((_) => new RuleResult(result, error));
            return rule;
        }

        public static IRule<TIn, RuleResult> ReturnElse<TIn>(this IRule<TIn, RuleResult> rule, bool result, FormError error = null) where TIn : class
        {
            rule.Else = RuleAction.FromFunc<TIn, RuleResult>((_) => new RuleResult(result, error));
            return rule;
        }

        #endregion


        public static IRuleAction<TIn, TOut> Build<TIn, TOut>(this IRule<TIn, TOut> rule) where TIn : class where TOut : class
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