using System;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FlatRent.BusinessRules.Builder.Extensions;
using FlatRent.Entities;
using Serilog;
using Serilog.Core;

namespace FlatRent.BusinessRules.Builder
{
    public class Playground
    {
        public static void Test()
        {
            var rule = new IfThenElseRule(RuleCondition.FromFunc(_ => 4 == 4),
                new IfThenElseRule(RuleCondition.FromFunc(
                    (o => ((string) o) == "asdf")), 
                    RuleAction.FromAct(o => Log.Information((string)o)), 
                    RuleAction.FromAct(_ => Log.Information("elsedeep"))),
                RuleAction.FromFunc(o =>
                {
                    Log.Information((string) o);
                    return "b";
                }));

            var rule2 = RuleBuilder
                .If(ob => ((Flat) ob).Price > 400)
                    .ThenIf(ob => ((Flat) ob).Price > 600)
                        .Then(ob => ((Flat) ob).Area = 100)
                    .ElseIf(ob => ((Flat) ob).Price < 550)
                        .Then(ob => ((Flat) ob).Area = 80)
                        .EndIf()
                    .EndIf()
                .Else(ob => ((Flat) ob).Area = 40)
            .Build();

            var usingOtherRule = RuleBuilder.If(ob => ((Flat) ob).Price > 300)
                .Then(rule2).Build();

            var fl = new Flat();
            fl.Price = 500;
            usingOtherRule.Execute(fl);
            Log.Information(fl.Area.ToString());
        }
    }
}