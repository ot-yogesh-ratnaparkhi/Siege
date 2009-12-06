using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions
{
    public static class RegistrationExtensions
    {
        public static IContextualServiceLocator serviceLocator;
        public static void Initialize(IContextualServiceLocator locator)
        {
            serviceLocator = locator;
        }

        public static IDecoratorUseCase<TDecorator> DecorateWith<TDecorator>(this IConditionalActivationRule rule)
        {
            var decoratorUseCase = new DecoratorUseCase<TDecorator>(serviceLocator);

            decoratorUseCase.BindTo(rule.GetBoundType());
            decoratorUseCase.SetActivationRule(rule);
            
            return decoratorUseCase;
        }
    }
}
