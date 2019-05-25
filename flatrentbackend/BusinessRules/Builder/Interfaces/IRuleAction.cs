using System;
using System.ComponentModel;

namespace FlatRent.BusinessRules.Builder.Interfaces
{
    public interface IRuleAction<in TIn, out TOut> where TIn : class where TOut : class
    {
        TOut Execute(TIn ob);
    }
}