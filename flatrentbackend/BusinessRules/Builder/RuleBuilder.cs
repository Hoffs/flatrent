using System;
using System.Linq;
using FlatRent.BusinessRules.Builder.Interfaces;
using FlatRent.BusinessRules.Inference;
using FlatRent.BusinessRules.Inference.Facts;

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

        public static IRule<TIn, TOut> IfInferHas<TIn, TOut>(params TermFact<TIn>[] facts) where TIn : class where TOut : class
        {
            return new IfThenElseRule<TIn, TOut>(null, 
                RuleCondition.FromFunc<TIn>(
                    ob =>
                    {
                        var infered = Infer.GetInfer<TIn>().Infer(ob);
                        return facts.All(f => infered.Has(f));
                    }
                ), null, null);
        }
    }
}