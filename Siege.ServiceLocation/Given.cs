using System;

namespace Siege.ServiceLocation
{
    public class Given<TBaseType>
    {
        public static ConditionalActivationRule<TBaseType, TContext> When<TContext>(Func<TContext, bool> evaluation)
        {
            return new ConditionalActivationRule<TBaseType, TContext>(evaluation);
        }

        public static DefaultUseCase<TBaseType> Then<TImplementingType>() where TImplementingType : TBaseType
        {
            DefaultUseCase<TBaseType> useCase = new DefaultUseCase<TBaseType>();

            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public static KeyBasedUseCase<TBaseType> Then<TImplementingType>(string key) where TImplementingType : TBaseType
        {
            KeyBasedUseCase<TBaseType> useCase = new KeyBasedUseCase<TBaseType>(key);

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
