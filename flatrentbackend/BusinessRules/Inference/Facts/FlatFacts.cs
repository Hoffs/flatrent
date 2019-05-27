using System;
using FlatRent.BusinessRules.Inference.Interfaces;
using FlatRent.Entities;

namespace FlatRent.BusinessRules.Inference.Facts
{
    public class FlatFacts : IInferer<Flat>
    {
        public FactBag<Flat> Infer(Flat flat)
        {
            var bag = new FactBag<Flat>();
            Inference.Infer.InferAll(bag, flat, 
                InferOwner, 
                InferActiveAgreement);
            return bag;
        }

        private static TermFact<Flat> InferOwner(FactBag<Flat> facts, Flat flat) => new TermFact<Flat>($"OWNER {flat.AuthorId}");
        private static TermFact<Flat> InferActiveAgreement(FactBag<Flat> facts, Flat flat) =>
            flat.ActiveAgreement != null ? new TermFact<Flat>($"HAS ACTIVE AGREEMENT") : null;

        public static TermFact<Flat> OwnerFact(Guid id) => new TermFact<Flat>($"OWNER {id}");
        public static TermFact<Flat> ActiveAgreementFact() => new TermFact<Flat>($"HAS ACTIVE AGREEMENT");
    }
}