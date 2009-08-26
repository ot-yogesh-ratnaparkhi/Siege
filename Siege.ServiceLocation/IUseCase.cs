using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase {}

    public interface IUseCase<TBaseType> : IUseCase
    {
        
    }

    public interface IUseCase<TBaseType, TOutput> : IUseCase<TBaseType>
    {
        TOutput Resolve<TContext>(TContext context) where TContext : IContext;
    }

    public abstract class UseCase<TBaseType, TOutput> : IUseCase<TBaseType, TOutput>
    {
        private List<IActivationRule> rules = new List<IActivationRule>();

        public abstract TOutput GetBinding();

        public void AddActivationRule(IActivationRule rule)
        {
            rules.Add(rule);
        }

        public TOutput Resolve<TContext>(TContext context)
            where TContext : IContext
        {
            foreach (ActivationRule<TBaseType, TContext> rule in this.rules)
            {
                if (rule.Evaluate(context)) return this.GetBinding();
            }

            return default(TOutput);
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
    }
}
