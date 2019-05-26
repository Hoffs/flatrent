using System;
using System.Linq.Expressions;

namespace FlatRent.BusinessRules.Inference.Terms
{
    public class SBVRRule<TATerm>
    {
        public SBVRRule(object @operator, TATerm aTerm, object bTerm)
        {
            Operator = @operator;
            ATerm = aTerm;
        }

        public TATerm ATerm { get; set; }
        public Expression<Func<TATerm, object>> MemberToUpdate { get; set; }
        public Func<TATerm, bool> HasAction { get; set; }
        public object Operator { get; set; }
    }

    public class Rule
    {
        public static SBVRRule<TIn> ItIsMandatoryThat<TIn>() where TIn : class
        {
            return new SBVRRule<TIn>(null, null, null);
        }
    }

    public static class SBVRRuleExtensions
    {
        public static SBVRRule<TIn> Has<TIn>(this SBVRRule<TIn> rule, Func<TIn, bool> has)
        {
            rule.HasAction = has;
            return rule;
        }

        public static SBVRRule<TIn> Property<TIn>(this SBVRRule<TIn> rule, Expression<Func<TIn, object>> memberToCheck)
        {
            rule.MemberToUpdate = memberToCheck;
//            rule.HasAction = has;
            return rule;
        }

        public static SBVRRule<TIn> IsMoreThan<TIn, TMember>(this SBVRRule<TIn> rule, Expression<Func<TIn, TMember>> memberToUpdate)
        {
//            rule.HasAction = has;
            return rule;
        }

        public static SBVRRule<TIn> Is<TIn>(this SBVRRule<TIn> rule, object value)
        {
//            rule.HasAction = has;
            return rule;
        }
    }
}