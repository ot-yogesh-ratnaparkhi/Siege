using System;

namespace Siege.ServiceLocation
{
    public class ConditionalActivationRule<TBaseType, TContext> : IActivationRule
    {
        private readonly Predicate<object> evaluation;

        public ConditionalActivationRule(Func<TContext, bool> evaluation)
        {
            this.evaluation = x => (x is TContext) ? evaluation.Invoke((TContext)x) : false;
        }

        public IConditionalUseCase<TBaseType> Then<TImplementingType>() where TImplementingType : TBaseType
        {
            ConditionalGenericUseCase<TBaseType> useCase = new ConditionalGenericUseCase<TBaseType>();

            useCase.AddActivationRule(this);
            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public IConditionalUseCase<TBaseType> Then(TBaseType implementation)
        {
            ConditionalImplementationUseCase<TBaseType> useCase = new ConditionalImplementationUseCase<TBaseType>();

            useCase.AddActivationRule(this);
            useCase.BindTo(implementation);

            return useCase;
        }

        public bool Evaluate(object context)
        {
            return evaluation.Invoke(context);
        }
    }
}