﻿using System;
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
//            var rule = new IfThenElseRule(RuleCondition.FromFunc(_ => 4 == 4),
//                new IfThenElseRule(RuleCondition.FromFunc(
//                    (o => ((string) o) == "asdf")), 
//                    RuleAction.FromAct(o => Log.Information((string)o)), 
//                    RuleAction.FromAct(_ => Log.Information("elsedeep"))),
//                RuleAction.FromFunc(o =>
//                {
//                    Log.Information((string) o);
//                    return "b";
//                }));

            var rule2 = RuleBuilder
                .If<Flat, RuleResult>(ob => ob.Price > 400)
                    .Do(ob => ob.Price = 900)
                    .ThenIf(ob => ob.Price > 600)
                        .Then(ob => ob.Area = 100)
                    .ElseIf(ob => ob.Price < 550)
                        .Then(ob => ob.Area = 80)
                        .EndIf()
                    .EndIf()
                .Else(ob => ob.Area = 40)
            .Build();

            var usingOtherRule = RuleBuilder.If<Flat, RuleResult>(ob => ob.Price > 300)
                .Then(rule2).Build();

            var fl = new Flat();
            fl.Price = 500;
            var rr = usingOtherRule.Execute(fl);
            Log.Information(fl.Area.ToString());
        }

        public static void DoStuff(Flat ob)
        {
            if (ob.Price > 300)
            {
                if (ob.Price > 400)
                {
                    ob.Price = 900;
                    if (ob.Price > 600)
                    {
                        ob.Area = 100;
                    }
                    else if (ob.Price < 550)
                    {
                        ob.Area = 80;
                    }
                } 
                else
                {
                    ob.Area = 40;
                }
            }
        }
    }
}