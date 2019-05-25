namespace FlatRent.BusinessRules.Builder.Interfaces
{
    public interface IRuleCondition
    {
        bool IsTrue(object ob);
    }
}