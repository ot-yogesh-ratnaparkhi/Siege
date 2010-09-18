using System;
using System.Collections.Generic;
using Siege.Requisitions.ResolutionRules;

namespace Siege.Requisitions.Extensions.MultiConditionalActivation
{
    public class MultiConditionalEvaluationStrategy : ContextEvaluationStrategy
    {
        private List<IConditionalActivationRule> list;

        public MultiConditionalEvaluationStrategy(List<IConditionalActivationRule> list)
        {
            this.list = list;
        }

        public override bool IsValid(IActivationRule rule, InternalStorage.IServiceLocatorStore context)
        {
            var items = MergeContextItems(context);

            for (int i = 0; i < items.Count; i++)
            {
                var contextItem = items[i];

                if (!EvaluateRules(contextItem)) return false;
            }

            return true;
        }

        private bool EvaluateRules(object contextItem)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j].Evaluate(contextItem)) return true;
            }
            return false;
        }
    }
}