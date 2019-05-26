using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlatRent.BusinessRules.Builder.Interfaces;
using FlatRent.Entities;

namespace FlatRent.BusinessRules.Inference.Terms
{
    public static class FlatFacts
    {
        public static bool InferHas(this Flat flat, TermFact<Flat> fact)
        {
            var bag = Infer(flat);
            return bag.Facts.Contains(fact);
        }

        public static FactBag<Flat> Infer(Flat flat)
        {
            var bag = new FactBag<Flat>();
            InferAll(bag, flat);
            return bag;
        }

        private static void InferAll(FactBag<Flat> fact, Flat flat)
        {
            var inferResults = new List<bool>();
            do
            {
                inferResults.Add(fact.AddFact(InferOwner(flat)));
                inferResults.Add(fact.AddFact(InferActiveAgreement(flat)));
            } while(inferResults.Any(i => i));

        }

        static TermFact<Flat> InferOwner(Flat flat) => new TermFact<Flat>($"OWNER {flat.AuthorId}");
        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");

        static TermFact<Flat> InferActiveAgreement(Flat flat) =>
            flat.ActiveAgreement != null ? new TermFact<Flat>($"HAS ACTIVE AGREEMENT") : null;
        static TermFact<Flat> ActiveAgreementFact() => new TermFact<Flat>($"HAS ACTIVE AGREEMENT");

//        static TermFact<Flat> InferOwner(Flat flat) => new TermFact<Flat>($"OWNER {flat.AuthorId}");
//        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
//
//        static TermFact<Flat> InferOwner(Flat flat) => new TermFact<Flat>($"OWNER {flat.AuthorId}");
//        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
//
//        static TermFact<Flat> InferOwner(Flat flat) => new TermFact<Flat>($"OWNER {flat.AuthorId}");
//        static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
    }
}