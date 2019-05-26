namespace FlatRent.BusinessRules.Inference.Interfaces
{
    public interface IInferer<TTerm>
    {
        FactBag<TTerm> Infer(TTerm term);
    }
}