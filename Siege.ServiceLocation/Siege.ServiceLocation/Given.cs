using System;

namespace Siege.ServiceLocation
{
    public class Given<TBaseService> 
    {
        public static ConditionalActivationRule<TBaseService, TContext> When<TContext>(Func<TContext, bool> evaluation)
        {
            return new ConditionalActivationRule<TBaseService, TContext>(evaluation);
        }

        public static IDefaultUseCase<TBaseService> Then<TImplementingType>() where TImplementingType : TBaseService
        {
            DefaultUseCase<TBaseService> useCase = new DefaultUseCase<TBaseService>();

            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public static IKeyBasedUseCase<TBaseService> Then<TImplementingType>(string key) where TImplementingType : TBaseService
        {
            KeyBasedUseCase<TBaseService> useCase = new KeyBasedUseCase<TBaseService>(key);

            useCase.BindTo<TImplementingType>();

            return useCase;
        }

        public static IDefaultUseCase<TBaseService> Then(TBaseService implementation)
        {
            DefaultInstanceUseCase<TBaseService> useCase = new DefaultInstanceUseCase<TBaseService>();

            useCase.BindTo(implementation);

            return useCase;
        }

        public static IKeyBasedUseCase<TBaseService> Then(string key, TBaseService implementation)
        {
            KeyBasedInstanceUseCase<TBaseService> useCase = new KeyBasedInstanceUseCase<TBaseService>(key);

            useCase.BindTo(implementation);

            return useCase;
        }
    }
}
