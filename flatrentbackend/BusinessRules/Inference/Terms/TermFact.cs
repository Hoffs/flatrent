namespace FlatRent.BusinessRules.Inference.Terms
{
    public class TermFact<TTerm>
    {
        public string Fact { get; private set; }

        public TermFact(string fact)
        {
            Fact = fact;
        }

        public override bool Equals(object obj)
        {
            return ((TermFact<TTerm>)obj).Fact == Fact;
        }
    }
}