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

    public class ActivationRule<TBaseType, TContext> : IActivationRule<TContext> where TContext :IContext
    {
        private readonly Func<TContext, bool> evaluation;

        public ActivationRule(Func<TContext, bool> evaluation)
        {
            this.evaluation = evaluation;
        }

        public IUseCase<TBaseType> Then<TImplementingType>() where TImplementingType : TBaseType
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
