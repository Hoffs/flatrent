using System;
using AutoMapper.Mappers;
using FlatRent.BusinessRules.Builder.Interfaces;

namespace FlatRent.BusinessRules.Builder
{
    public static class RuleAction
    {

        public static IRuleAction<TIn, TOut> FromFunc<TIn, TOut>(Func<TIn, TOut> ob) where TIn : class where TOut : class
        {
            return new SimpleRuleAction<TIn, TOut>(ob);
        }

        public static IRuleAction<TIn, TOut> FromAct<TIn, TOut>(Action<TIn> ob) where TIn : class where TOut : class
        {
            return new SimpleRuleAction<TIn, TOut>((o) => {
                ob(o);
                return null; 
            });
        }

        public static IRuleAction<TIn, TOut> DoNothing<TIn, TOut>() where TIn : class where TOut : class
            => FromAct<TIn, TOut>((ob) => {});

        class SimpleRuleAction<TIn, TOut> : IRuleAction<TIn, TOut> where TIn : class where TOut : class
        {
            private readonly Func<TIn, TOut> _ob;

            public SimpleRuleAction(Func<TIn, TOut> ob)
            {
                _ob = ob;
            }

            public TOut Execute(TIn ob)
            {
                return _ob(ob);
            }
        }
    }
}