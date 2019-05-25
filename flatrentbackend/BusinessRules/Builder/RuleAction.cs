using System;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public static class RuleAction
    {

        public static IRuleAction FromFunc(Func<object, object> ob)
        {
            return new SimpleRuleAction(ob);
        }

        public static IRuleAction FromAct(Action<object> ob)
        {
            return new SimpleRuleAction((o) => { ob(o);
                return null; 
            });
        }

        public static IRuleAction DoNothing = FromAct((ob) => {});

        class SimpleRuleAction : IRuleAction
        {
            private readonly Func<object, object> _ob;

            public SimpleRuleAction(Func<object, object> ob)
            {
                _ob = ob;
            }

            public object Execute(object ob)
            {
                return _ob(ob);
            }
        }
    }
}