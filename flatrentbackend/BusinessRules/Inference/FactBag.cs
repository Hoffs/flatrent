using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using FlatRent.BusinessRules.Inference.Facts;

namespace FlatRent.BusinessRules.Inference
{
    public class FactBag<TTerm>
    {
        private readonly List<TermFact<TTerm>> _facts = new List<TermFact<TTerm>>();
        public IReadOnlyCollection<TermFact<TTerm>> Facts => _facts.AsReadOnly();

        public FactBag()
        {
        }

        public bool AddFact(TermFact<TTerm> fact)
        {
            if (fact == null) return false;

            if (_facts.Contains(fact))
            {
                return false;
            }

            _facts.Add(fact);
            return true;
        }

        public bool Has(TermFact<TTerm> fact)
        {
            return _facts.Contains(fact);
        }
    }
}