using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public class IfThenElseRule : IRule
    {
        public IRule Parent { get; }
        public IRuleCondition If { get; set; }
        public IRuleAction Then { get; set; }
        public IRuleAction Else { get; set; }

        public IfThenElseRule(IRule parent, IRuleCondition @if, IRuleAction then, IRuleAction @else)
        {
            Parent = parent;
            If = @if;
            Then = then;
            Else = @else;
        }

        public IfThenElseRule(IRuleCondition @if, IRuleAction then, IRuleAction @else)
        {
            If = @if;
            Then = then;
            Else = @else;
        }

        public IfThenElseRule(IRule parent)
        {
            Parent = parent;
        }

        public object Execute(object o)
        {
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