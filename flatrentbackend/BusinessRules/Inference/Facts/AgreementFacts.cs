using FlatRent.BusinessRules.Inference.Interfaces;
using FlatRent.Constants;
using FlatRent.Entities;

namespace FlatRent.BusinessRules.Inference.Facts
{
    public class AgreementFacts : IInferer<Agreement>
    {
        public FactBag<Agreement> Infer(Agreement agreement)
        {
            var bag = new FactBag<Agreement>();
            Inference.Infer.InferAll(bag, agreement, 
                InferExceedsMaxPeriod, 
                InferTooShortPeriod, 
                InferOwner, 
                InferNotOwner);
            return bag;
        }

        private static TermFact<Agreement> InferExceedsMaxPeriod(Agreement agreement) 
            => agreement.RentPeriodDays > BusinessConstants.MaxRentPeriodDays ? new TermFact<Agreement>("LONGER THAN MAX PERIOD") : null;
        private static TermFact<Agreement> InferTooShortPeriod(Agreement agreement) 
            => agreement.RentPeriodDays < agreement.Flat.MinimumRentDays ? new TermFact<Agreement>("SHORTER THAN MIN PERIOD") : null;
        private static TermFact<Agreement> InferOwner(Agreement agreement) 
            => agreement.AuthorId == agreement.Flat.AuthorId ? new TermFact<Agreement>($"TENANT IS OWNER") : null;
        private static TermFact<Agreement> InferNotOwner(Agreement agreement) 
            => agreement.AuthorId != agreement.Flat.AuthorId ? new TermFact<Agreement>($"TENANT IS NOT OWNER") : null;
        
        public static TermFact<Agreement> TenantIsOwner => new TermFact<Agreement>($"TENANT IS OWNER");
        public static TermFact<Agreement> TenantIsNotOwner => new TermFact<Agreement>($"TENANT IS NOT OWNER");
        public static TermFact<Agreement> TooShortPeriod => new TermFact<Agreement>($"SHORTER THAN MIN PERIOD");
        public static TermFact<Agreement> ExceedsMaxPeriod => new TermFact<Agreement>($"LONGER THAN MAX PERIOD");
    }
}