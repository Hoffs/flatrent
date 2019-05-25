namespace FlatRent.BusinessRules.Builder.Interfaces
{
    public interface IRuleCondition<in TIn>
    {
        bool IsTrue(TIn ob);
    }
}