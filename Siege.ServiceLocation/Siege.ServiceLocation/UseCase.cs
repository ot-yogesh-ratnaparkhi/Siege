using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public abstract class UseCase<TBaseService, TService> : IUseCase<TBaseService>
    {
        protected IActivationRule rule;
        public abstract TService GetBinding();
        public abstract Type GetUseCaseBindingType();
        public abstract Type GetBaseBindingType();
        protected abstract IActivationStrategy GetActivationStrategy();

        public void SetActivationRule(IActivationRule rule)
        {
            this.rule = rule;
        }

        public bool IsValid(IList<object> context)
        {
            foreach (object contextItem in context)
            {
                if (rule.Evaluate(contextItem)) return true;
            }

            return false;
        }

        public virtual object Resolve(IInstanceResolver locator, IList<object> context) 
        {
            if(IsValid(context)) return GetActivationStrategy().Resolve(locator, context);

            return default(TBaseService);
        }

        public object Resolve(IInstanceResolver locator)
        {
            return GetActivationStrategy().Resolve(locator, null);
        }

        public abstract Type GetBoundType();
    }

    public interface IActivationStrategy
    {
        object Resolve(IInstanceResolver locator, IList<object> context);
    }
}