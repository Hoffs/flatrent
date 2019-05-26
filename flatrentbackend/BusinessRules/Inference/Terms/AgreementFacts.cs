using System;
using System.Collections.Generic;
using System.Linq;
using FlatRent.BusinessRules.Builder;
using FlatRent.BusinessRules.Builder.Extensions;
using FlatRent.BusinessRules.Builder.Interfaces;
using FlatRent.BusinessRules.Inference.Interfaces;
using FlatRent.Constants;
using FlatRent.Entities;

namespace FlatRent.BusinessRules.Inference.Terms
{
    public class AgreementFacts : IInferer<Agreement>
    {

        #region Extensions

//        public static bool InferHas(this Agreement flat, TermFact<Agreement> agreement)
//        {
//            var bag = Infer(flat);
//            return bag.Facts.Contains(agreement);
//        }
//
//        public static IRule<Agreement, TOut> IfInferHas<TOut>(this IRule<Agreement, TOut> rule, TermFact<Agreement> fact) where TOut : class
//        {
//            rule.If(RuleCondition.FromFunc<Agreement>((a) => a.InferHas(fact)));
//            return rule;
//        }

        #endregion

        public FactBag<Agreement> Infer(Agreement agreement)
        {
            var bag = new FactBag<Agreement>();
            InferAll(bag, agreement);
            return bag;
        }

        private static void InferAll(FactBag<Agreement> fact, Agreement agreement)
        {
            var inferResults = new List<bool>();
            do
            {
                inferResults.Add(fact.AddFact(InferExceedsMaxPeriod(agreement)));
                inferResults.Add(fact.AddFact(InferTooShortPeriod(agreement)));
                inferResults.Add(fact.AddFact(InferOwner(agreement)));
            } while (inferResults.Any(i => i));

        }

        private static TermFact<Agreement> InferExceedsMaxPeriod(Agreement agreement) 
            => agreement.RentPeriodDays > BusinessConstants.MaxRentPeriodDays ? new TermFact<Agreement>("LONGER THAN MAX PERIOD") : null;
        private static TermFact<Agreement> InferTooShortPeriod(Agreement agreement) 
            => agreement.RentPeriodDays < agreement.Flat.MinimumRentDays ? new TermFact<Agreement>("SHORTER THAN MIN PERIOD") : null;
        private static TermFact<Agreement> InferOwner(Agreement agreement) 
            => agreement.AuthorId == agreement.Flat.AuthorId ? new TermFact<Agreement>($"TENANT IS OWNER") : null;
        
        public static TermFact<Agreement> TenantIsOwner => new TermFact<Agreement>($"TENANT IS OWNER");
        public static TermFact<Agreement> TooShortPeriod => new TermFact<Agreement>($"SHORTER THAN MIN PERIOD");
        public static TermFact<Agreement> ExceedsMaxPeriod => new TermFact<Agreement>($"LONGER THAN MAX PERIOD");

        //        static TermFact<Flat> InferOwner(Flat agreement) => new TermFact<Flat>($"OWNER {agreement.AuthorId}");
        //        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
        //
        //        static TermFact<Flat> InferOwner(Flat agreement) => new TermFact<Flat>($"OWNER {agreement.AuthorId}");
        //        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
        //
        //        static TermFact<Flat> InferOwner(Flat agreement) => new TermFact<Flat>($"OWNER {agreement.AuthorId}");
        //        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
    }
}