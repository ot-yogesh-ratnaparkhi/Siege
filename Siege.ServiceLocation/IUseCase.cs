using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase { }

    public interface IUseCase<TBaseType> : IUseCase
    {
        TBaseType Resolve<TContext>(IServiceLocator locator, TContext context, IDictionary dictionary);
    }

    public abstract class UseCase<TBaseType, TOutput> : IUseCase<TBaseType>
    {
        private readonly List<IActivationRule> rules = new List<IActivationRule>();
        public abstract TOutput GetBinding();
        protected abstract IActivationStrategy GetActivationStrategy();

        public void AddActivationRule(IActivationRule rule)
        {
            rules.Add(rule);
        }

        public TBaseType Resolve<TContext>(IServiceLocator locator, TContext context, IDictionary constructorArguments)
        {
            foreach (IActivationRule<TContext> rule in this.rules)
            {
                if (rule.Evaluate(context)) return GetActivationStrategy().Resolve(locator, constructorArguments);
            }

            return default(TBaseType);
        }

        protected interface IActivationStrategy
        {
            TBaseType Resolve(IServiceLocator locator, IDictionary constructorArguments);
        }
    }

    public class GenericUseCase<TBaseType> : UseCase<TBaseType, Type>
    {
        private Type boundType;

        public void BindTo<TImplementationType>()
        {
            boundType = typeof(TImplementationType);
        }

        public override Type GetBinding()
        {
            return boundType;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new GenericActivationStrategy(boundType);
        }

        public class GenericActivationStrategy : IActivationStrategy
        {
            private readonly Type boundType;

            public GenericActivationStrategy(Type boundType)
            {
                this.boundType = boundType;
            }

            public TBaseType Resolve(IServiceLocator locator, IDictionary constructorArguments)
            {
                return locator.GetInstance<TBaseType>(boundType, constructorArguments);
            }
        }
    }

    public class ImplementationUseCase<TBaseType> : UseCase<TBaseType, TBaseType>
    {
        private TBaseType implementation;

        public void BindTo(TBaseType implementation)
        {
            this.implementation = implementation;
        }

        public override TBaseType GetBinding()
        {
            return this.implementation;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new ImplementationActivationStrategy(implementation);
        }

        public class ImplementationActivationStrategy : IActivationStrategy
        {
            private readonly TBaseType implementation;

            public ImplementationActivationStrategy(TBaseType implementation)
            {
                this.implementation = implementation;
            }

            public TBaseType Resolve(IServiceLocator locator, IDictionary constructorArguments)
            {
                return implementation;
            }
        }
    }

    public class KeyBasedUseCase<TBaseType> : GenericUseCase<TBaseType>
    {
        private readonly string key;

        public KeyBasedUseCase(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return key; }
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new KeyBasedActivationStrategy(Key);
        }

        public class KeyBasedActivationStrategy : IActivationStrategy
        {
            private readonly string key;

            public KeyBasedActivationStrategy(string key)
            {
                this.key = key;
            }

            public TBaseType Resolve(IServiceLocator locator, IDictionary constructorArguments)
            {
                return locator.GetInstance<TBaseType>(key, constructorArguments);
            }
        }
    }

    public class DefaultUseCase<TBaseType> : GenericUseCase<TBaseType>
    {

    }
}