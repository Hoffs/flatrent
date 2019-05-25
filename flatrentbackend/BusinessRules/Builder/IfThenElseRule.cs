using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public class IfThenElseRule<TIn, TOut> : IRule<TIn, TOut> where TIn : class where TOut : class
    {
        public IRule<TIn, TOut> Parent { get; }
        public IRuleCondition<TIn> If { get; set; }
        public Action<TIn> DoAction { get; set; }
        public IRuleAction<TIn, TOut> Then { get; set; }
        public IRuleAction<TIn, TOut> Else { get; set; }

        public IfThenElseRule(IRule<TIn, TOut> parent, IRuleCondition<TIn> @if, IRuleAction<TIn, TOut> then, IRuleAction<TIn, TOut> @else)
        {
            Parent = parent;
            If = @if;
            Then = then;
            Else = @else;
        }

        public IfThenElseRule(IRuleCondition<TIn> @if, IRuleAction<TIn, TOut> then, IRuleAction<TIn, TOut> @else)
        {
            If = @if;
            Then = then;
            Else = @else;
        }

        public IfThenElseRule(IRule<TIn, TOut> parent)
        {
            Parent = parent;
        }

        public TOut Execute(TIn o)
        {
            DoAction?.Invoke(o);

            if (If == null) throw new InvalidOperationException("If was not set when executing Rule");
            if (If.IsTrue(o))
            {
                if (Then == null) throw new InvalidOperationException("Then was not set when executing Rule");
                return Then.Execute(o);
            }

            if (Else == null) throw new InvalidOperationException("Else was not set when executing Rule");
            return Else.Execute(o);
        }
    }
}