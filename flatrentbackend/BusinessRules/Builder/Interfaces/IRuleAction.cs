using System;
using System.ComponentModel;

namespace FlatRent.BusinessRules.Builder.Interfaces
{
    public interface IRuleAction
    {
        object Execute(object ob);
    }
}