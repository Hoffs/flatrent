using System;
using System.Linq;
using FlatRent.BusinessRules.Builder;
using FlatRent.BusinessRules.Builder.Extensions;
using FlatRent.BusinessRules.Builder.Interfaces;
using FlatRent.BusinessRules.Inference.Interfaces;
using FlatRent.BusinessRules.Inference.Terms;

namespace FlatRent.BusinessRules.Inference
{
    public static class Infer
    {
        public static IInferer<TTerm> GetInfer<TTerm>()
        {
            var type = typeof(IInferer<TTerm>);
            var inferer = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
            if (inferer == null) throw new InvalidOperationException($"No IInferer<{typeof(TTerm)}> found");
            return (IInferer<TTerm>) Activator.CreateInstance(inferer);
        }

        public static IRule<TIn, TOut> IfInferHas<TIn, TOut>(params TermFact<TIn>[] facts) where TIn : class where TOut : class
        {
            return new IfThenElseRule<TIn, TOut>(null,
                RuleCondition.FromFunc<TIn>(
                    ob =>
                    {
                        var infered = GetInfer<TIn>().Infer(ob);
                        return facts.All(f => infered.Has(f));
                    }
                ), null, null);
        }

        #region InferExtensions

        public static bool InferHas<TTerm>(this IInferer<TTerm> inferer, TTerm term, params TermFact<TTerm>[] facts)
        {
            var infered = inferer.Infer(term);
            return facts.All(f => infered.Has(f));
        }

        #endregion

        #region RuleExtensions

        public static IRule<TIn, TOut> IfInferHas<TIn, TOut>(this IRule<TIn, TOut> rule, params TermFact<TIn>[] facts) where TIn : class where TOut : class
        {
            var inferer = GetInfer<TIn>();
            rule.If(RuleCondition.FromFunc<TIn>(a => inferer.InferHas(a, facts)));
            return rule;
        }

        public static IRule<TIn, TOut> ThenIfInferHas<TIn, TOut>(this IRule<TIn, TOut> rule, params TermFact<TIn>[] facts) where TIn : class where TOut : class
        {
            var inferer = GetInfer<TIn>();
            var newRule = new IfThenElseRule<TIn, TOut>(rule);
            newRule.If(a => inferer.InferHas(a, facts));

            rule.Then(newRule);

            return newRule;
        }

        public static IRule<TIn, TOut> ElseIfInferHas<TIn, TOut>(this IRule<TIn, TOut> rule, params TermFact<TIn>[] facts) where TIn : class where TOut : class
        {
            var inferer = GetInfer<TIn>();
            var newRule = new IfThenElseRule<TIn, TOut>(rule);
            newRule.If(a => inferer.InferHas(a, facts));

            rule.Else(newRule);

            return newRule;
        }

        #endregion
    }
}