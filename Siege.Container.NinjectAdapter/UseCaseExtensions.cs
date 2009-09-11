using System.Collections;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;
using Siege.ServiceLocation;
using IContext=Ninject.Activation.IContext;

namespace Siege.Container.NinjectAdapter
{
    public static class UseCaseExtensions
    {
        public static void Bind<TBaseType>(this IConditionalUseCase<TBaseType> useCase, IKernel kernel, NinjectAdapter locator)
        {
            var factory = (NinjectFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            kernel.Bind<TBaseType>().ToMethod(context => factory.Build(new ParameterAdapter(context).Dictionary));
            kernel.Bind(useCase.GetBoundType()).ToSelf();
        }

        public static void Bind<TBaseType>(this IDefaultUseCase<TBaseType> useCase, IKernel kernel, NinjectAdapter locator)
        {
            var factory = (NinjectFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            if (typeof(TBaseType) != useCase.GetBoundType()) kernel.Bind<TBaseType>().ToMethod(context => factory.Build(new ParameterAdapter(context).Dictionary));
            kernel.Bind(useCase.GetBoundType()).ToSelf();
        }
        
        public static void Bind<TBaseType>(this DefaultImplementationUseCase<TBaseType> useCase, IKernel kernel, NinjectAdapter locator)
        {
            var factory = (NinjectFactory<TBaseType>)locator.GetFactory<TBaseType>();
            factory.AddCase(useCase);

            kernel.Bind<TBaseType>().ToConstant(useCase.GetBinding());
        }
        
        public static void Bind<TBaseType>(this IKeyBasedUseCase<TBaseType> useCase, IKernel kernel)
        {
            kernel.Bind<TBaseType>().To(useCase.GetBoundType()).Named(useCase.Key);
        }
    }

    public class ParameterAdapter
    {
        public ParameterAdapter(IContext context)
        {
            Dictionary = new Dictionary<string, object>();
            if (context.Parameters == null) return;
            foreach (IParameter parameter in context.Parameters)
            {
                var value = parameter.GetValue(context);
                if (value == null) continue;
                Dictionary.Add(parameter.Name, value);
            }
        }

        public IDictionary Dictionary { get; set; }
    }
}