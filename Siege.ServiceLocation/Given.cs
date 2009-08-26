using System;

namespace Siege.ServiceLocation
{
    public class Given<TBaseType>
    {
        public static ActivationRule<TBaseType, TContext> When<TContext>(Func<TContext, bool> evaluation)
            where TContext : IContext
        {
            return new ActivationRule<TBaseType, TContext>(evaluation);
        }

        public static IUseCase<TBaseType> Then<TImplementingType>() where TImplementingType : TBaseType
        {
            GenericUseCase<TBaseType> useCase = new GenericUseCase<TBaseType>();

            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public static IUseCase<TBaseType> Then(TBaseType implementation)
        {
            ImplementationUseCase<TBaseType> useCase = new ImplementationUseCase<TBaseType>();

            useCase.BindTo(implementation);

            return useCase;
        }
    }
}
