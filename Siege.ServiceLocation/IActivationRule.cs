using System;

namespace Siege.ServiceLocation
{
    public interface IActivationRule
    {
    }

    public interface IActivationRule<TContext> : IActivationRule
    {   
        bool Evaluate(TContext context);
    }

    public class ConditionalActivationRule<TBaseType, TContext> : IActivationRule<TContext>
    {
        private readonly Func<TContext, bool> evaluation;

        public ConditionalActivationRule(Func<TContext, bool> evaluation)
        {
            this.evaluation = evaluation;
        }

        public GenericUseCase<TBaseType> Then<TImplementingType>() where TImplementingType : TBaseType
        {
            GenericUseCase<TBaseType> useCase = new GenericUseCase<TBaseType>();

            useCase.AddActivationRule(this);
            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public bool Evaluate(TContext context)
        {
            return evaluation.Invoke(context);
        }
    }
}
