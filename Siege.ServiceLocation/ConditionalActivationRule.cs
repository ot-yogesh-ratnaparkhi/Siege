using System;

namespace Siege.ServiceLocation
{
    public class ConditionalActivationRule<TBaseService, TContext> : IActivationRule
    {
        private readonly Predicate<object> evaluation;

        public ConditionalActivationRule(Func<TContext, bool> evaluation)
        {
            this.evaluation = x => (x is TContext) ? evaluation.Invoke((TContext)x) : false;
        }

        public IConditionalUseCase<TBaseService> Then<TImplementingType>() where TImplementingType : TBaseService
        {
            ConditionalGenericUseCase<TBaseService> useCase = new ConditionalGenericUseCase<TBaseService>();

            useCase.AddActivationRule(this);
            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public IConditionalUseCase<TBaseService> Then(TBaseService implementation)
        {
            ConditionalInstanceUseCase<TBaseService> useCase = new ConditionalInstanceUseCase<TBaseService>();

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