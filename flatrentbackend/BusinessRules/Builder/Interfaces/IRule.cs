namespace FlatRent.BusinessRules.Builder.Interfaces
{
    public interface IRule : IRuleAction
    {
        IRule Parent { get; }
        IRuleCondition If { get; set; }
        IRuleAction Then { get; set; }
        IRuleAction Else { get; set; }
    }
}