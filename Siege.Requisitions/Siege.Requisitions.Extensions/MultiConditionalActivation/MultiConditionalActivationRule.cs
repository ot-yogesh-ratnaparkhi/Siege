using System;
using System.Collections.Generic;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.ResolutionRules;

namespace Siege.Requisitions.Extensions.MultiConditionalActivation
{
    public class MultiConditionalActivationRule<TService> : ActivationRule<TService, object>
    {
        private readonly List<IConditionalActivationRule> list = new List<IConditionalActivationRule>();

        public void When<TContext>(Func<TContext, bool> evaluation)
        {
            var rule = new ExtendedRegistrationSyntax.ConditionalActivationRule<TService, TContext>();

            rule.SetEvaluation(evaluation);

            list.Add(rule);
        }

        public override IRuleEvaluationStrategy GetRuleEvaluationStrategy()
        {
            return new MultiConditionalEvaluationStrategy(list);
        }
    }
}