using System;

namespace FlatRent.BusinessRules.Builder.Interfaces
{
    public interface IRule<TIn, TOut> : IRuleAction<TIn, TOut> where TIn : class where TOut : class
    {
        IRule<TIn, TOut> Parent { get; }
        IRuleCondition<TIn> If { get; set; }
        Action<TIn> DoAction { get; set; }
        IRuleAction<TIn, TOut> Then { get; set; }
        IRuleAction<TIn, TOut> Else { get; set; }
    }
}