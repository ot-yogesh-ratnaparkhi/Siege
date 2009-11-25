using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public abstract class UseCase<TBaseService, TService> : IUseCase<TBaseService>
    {
        protected readonly List<IActivationRule> rules = new List<IActivationRule>();
        public abstract TService GetBinding();
        protected abstract IActivationStrategy GetActivationStrategy();

        public void AddActivationRule(IActivationRule rule)
        {
            rules.Add(rule);
        }

        public object Resolve(IMinimalServiceLocator locator, IList<object> context, IDictionary constructorArguments) 
        {
            foreach (IActivationRule rule in this.rules)
            {
                foreach (object contextItem in context)
                {
                    if (rule.Evaluate(contextItem)) return GetActivationStrategy().Resolve(locator, constructorArguments);
                }
            }

            return default(TBaseService);
        }

        public object Resolve(IMinimalServiceLocator locator, IDictionary constructorArguments)
        {
            return GetActivationStrategy().Resolve(locator, constructorArguments);
        }

        public abstract Type GetBoundType();

        protected interface IActivationStrategy
        {
            TBaseService Resolve(IMinimalServiceLocator locator, IDictionary constructorArguments);
        }
    }
}