using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public abstract class UseCase<TBaseService, TService> : IUseCase<TBaseService>
    {
        protected readonly List<IActivationRule> rules = new List<IActivationRule>();
        public abstract TService GetBinding();
        public abstract Type GetUseCaseBindingType();
        protected abstract IActivationStrategy<TBaseService> GetActivationStrategy();

        public void AddActivationRule(IActivationRule rule)
        {
            rules.Add(rule);
        }

        public virtual object Resolve(IInstanceResolver locator, IList<object> context) 
        {
            foreach (IActivationRule rule in rules)
            {
                foreach (object contextItem in context)
                {
                    if (rule.Evaluate(contextItem)) return GetActivationStrategy().Resolve(locator);
                }
            }

            return default(TBaseService);
        }

        public object Resolve(IInstanceResolver locator)
        {
            return GetActivationStrategy().Resolve(locator);
        }

        public abstract Type GetBoundType();
    }

    public interface IActivationStrategy<TService>
    {
        TService Resolve(IInstanceResolver locator);
    }
}